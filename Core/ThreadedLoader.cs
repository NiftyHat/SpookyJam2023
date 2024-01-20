using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Array = Godot.Collections.Array;

namespace SpookyBotanyGame.Core;

public partial class ThreadedLoader : Node
{
    public class LoadingList
    {
        protected class Loader : Loader<Resource>
        {
            public Loader(string path, Action<Loader<Resource>, Resource> onComplete, Action<Loader<Resource>, Exception> onFailed = null) : base(
                path, onComplete, onFailed)
            {
            }
        }

        protected class Loader<TResource> where TResource : Resource
        {
            protected string _path;
            protected double _percentage;
            public double Percentage => _percentage;
            public long Ticks => _ticks;
            protected bool _isRunning;
            protected event Action<Loader<TResource>, TResource> _onComplete;
            protected event Action<Loader<TResource>, Exception> _onFailed;
            protected int _retryCount;
            protected int _maxRetryCount = 5;
            protected long _ticks;
            protected Stopwatch _stopwatch;

            public Loader(string path, Action<Loader<TResource>, TResource> onComplete,
                Action<Loader<TResource>, Exception> onFailed = null)
            {
                _stopwatch = new Stopwatch();
                _path = path;
                _onComplete = onComplete;
                _onFailed = onFailed;
            }

            public void Run()
            {
                if (_isRunning)
                {
                    return;
                }

                _stopwatch.Start();
                _isRunning = true;
                if (ResourceLoader.HasCached(_path))
                {
                    Complete();
                }
            }

            protected void Complete()
            {
                TResource resource = ResourceLoader.LoadThreadedGet(_path) as TResource;
                _stopwatch.Stop();
                _ticks = _stopwatch.ElapsedTicks;
                if (resource == null)
                {
                    _onFailed?.Invoke(this,
                        new Exception(
                            $"{nameof(Loader<TResource>)} failed load asset '{_path}' cannot convert to type {typeof(TResource).Name}"));
                    _isRunning = false;
                    return;
                }

                _onComplete?.Invoke(this, resource);
                _isRunning = false;
            }

            public void Update()
            {
                Array progress = new Array();
                ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_path, progress);
                switch (status)
                {
                    case ResourceLoader.ThreadLoadStatus.InvalidResource:
                        if (_retryCount <= _maxRetryCount)
                        {
                            ResourceLoader.LoadThreadedRequest(_path);
                            _retryCount++;
                        }
                        else
                        {
                            _onFailed?.Invoke(this,
                                new AssetLoadException(
                                    $"{status} Path:'{_path}' after {_retryCount}/{_maxRetryCount} retries", _path,
                                    status));
                        }

                        return;
                    case ResourceLoader.ThreadLoadStatus.InProgress:
                        if (progress.Count > 0)
                        {
                            _percentage = (double)progress[0];
                        }
                        else
                        {
                            _percentage = 0;
                        }

                        return;
                    case ResourceLoader.ThreadLoadStatus.Failed:
                        _onFailed?.Invoke(this, new AssetLoadException(_path, status));
                        return;
                    case ResourceLoader.ThreadLoadStatus.Loaded:
                        _percentage = 1.0;
                        Complete();
                        return;
                    default:
                        _onFailed?.Invoke(this, new AssetLoadException(_path, status));
                        return;
                }
            }
        }
        
        protected List<Loader> _items;
        
        public LoadingList(string path)
        {
            _items = new List<Loader>()
            {
                new Loader(path, HandleAssetLoadingComplete, HandleAssetLoadingFailed),
            };
        }
        
        public LoadingList(IEnumerable<string> paths)
        {
            _items = new List<Loader>();
            foreach (var item in paths)
            {
                _items.Add(new Loader(item, HandleAssetLoadingComplete, HandleAssetLoadingFailed));
            }
        }

        private void HandleAssetLoadingComplete(Loader<Resource> loader, Resource resource)
        {
            
        }
        
        private void HandleAssetLoadingFailed(Loader<Resource> loader, Exception exception)
        {
        }

        public void Update()
        {
            foreach (var item in _items)
            {
                item.Update();
            }
        }

    }

    private LoadingList _loadingList;


    public override void _Process(double delta)
    {
        _loadingList.Update();
        base._Process(delta);
    }

    public class AssetLoadException : Exception
    {
        public readonly string AssetPath;
        public readonly ResourceLoader.ThreadLoadStatus Status;

        public AssetLoadException(string assetPath, ResourceLoader.ThreadLoadStatus status) : base(
            $"{status} Path:'{assetPath}'")
        {
            AssetPath = assetPath;
            Status = status;
        }

        public AssetLoadException(string message, string assetPath, ResourceLoader.ThreadLoadStatus status) :
            base(message)
        {
            AssetPath = assetPath;
        }
    }
}