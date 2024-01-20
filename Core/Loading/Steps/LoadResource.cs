using System;
using System.Threading.Tasks;
using Godot;
using Array = Godot.Collections.Array;

namespace SpookyBotanyGame.Core.Loading.Steps;

public class LoadResource<TResource> : LoadingStep where TResource : Godot.Resource
{
    protected string _path;
    protected string _typeHint;
    protected bool _useThreads = false;
    protected ResourceLoader.CacheMode _cacheMode = ResourceLoader.CacheMode.Reuse;
    protected ActionQueue _updateHandler;
    
    public LoadResource(int size, string name, string path, ActionQueue updateHandler) : base(size, name)
    {
        _path = path;
        _typeHint = typeof(TResource).Name;
        _updateHandler = updateHandler;
    }
    
    public override void Run(Action<LoadingStep> onComplete, IProgress<float> progress = null)
    {
        ResourceLoader.LoadThreadedRequest(_path, _typeHint, _useThreads, _cacheMode);

        void CheckForLoadComplete()
        {
            bool isComplete = CheckLoadThreadedState(progress);
            if (isComplete)
            {
                //_updateHandler.Remove(CheckForLoadComplete);
            }
        }
        _updateHandler.Add(CheckForLoadComplete);
        onComplete?.Invoke(this);
    }

    public bool CheckLoadThreadedState(IProgress<float> progress = null)
    {
        var status = LoadThreadedGetStatus(_path, out var percentage);
        switch (status)
        {
            case ResourceLoader.ThreadLoadStatus.InvalidResource:
                return false;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                progress?.Report(percentage);
                return false;
            case ResourceLoader.ThreadLoadStatus.Failed:
                return true;
            case ResourceLoader.ThreadLoadStatus.Loaded:
                return false;
            default:
                return false;
        }
    }

    public override Task GetTask(IProgress<float> progress = null)
    {
        ResourceLoader.LoadThreadedRequest(_path, _typeHint, _useThreads, _cacheMode);
        bool isComplete = false;
        while (!isComplete)
        {
            var status = LoadThreadedGetStatus(_path, out var percentage);
            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    return Task.FromException<TResource>(new Exception($"Asset at : {_path} is invalid."));
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    progress?.Report(percentage);
                    break;
                case ResourceLoader.ThreadLoadStatus.Failed:
                    isComplete = true;
                    break;
                case ResourceLoader.ThreadLoadStatus.Loaded:
                    isComplete = true;
                    break;
                default:
                    return Task.FromException<TResource>(new Exception($"Loading asset '{_path}' returned unhandled status {status}."));;
            }
        }
        return Task.CompletedTask;
    }

    public ResourceLoader.ThreadLoadStatus LoadThreadedGetStatus(string path, out float percentage)
    {
        Array progress = new Array();
        ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_path, progress);
        if (progress.Count > 0)
        {
            percentage = (float)progress[0];
        }
        else
        {
            percentage = 0;
        }
        return status;
    }
}