using System;
using System.Collections.Generic;
using System.Diagnostics;
using SpookyBotanyGame.Core.Loading.Steps;

namespace SpookyBotanyGame.Core.Loading
{
    public class LoadingQueue
    {
        public readonly Queue<LoadingStep> _queue = new();
        private int _totalSize;
        private int _completed;
        public bool IsRunning { get; private set; }

        public LoadingQueue()
        {
            LoadingStep startTask = new Wait(1, "Start", new TimeSpan(50000));
            Add(startTask);
        }

        public void Add(LoadingStep step)
        {
            _queue.Enqueue(step);
            _totalSize += step.Size;
        }

        public void Run(Queue<LoadingStep> queue, IProgress<LoadingQueueState> progress, Action<TimeSpan> OnComplete)
        {
            AppendEndTask();
            ProcessQueue(queue, progress);
        }

        public void ProcessQueue(Queue<LoadingStep> queue, IProgress<LoadingQueueState> progress)
        {
            int totalStepCount = _queue.Count;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (queue.Count > 0)
            {
                LoadingStep next = _queue.Dequeue();
                Progress<float> taskProgress = new Progress<float>();
                taskProgress.ProgressChanged += (sender, taskProgressPercentage) =>
                {
                    next.SetTaskProgress(taskProgressPercentage);
                };
                float percentage =  Math.Clamp((1.0f / _totalSize) * _completed, 0f, 1f);
                next.Run((step) =>
                {
                    int currentStep = totalStepCount - _queue.Count;
                    progress.Report(new LoadingQueueState()
                    {
                        Percentage = percentage,
                        Name = next.Name,
                        Current = currentStep,
                        Total = totalStepCount,
                        StartTick = stopwatch.ElapsedTicks
                    });
                    ProcessQueue(queue, progress);
                });
            }
            stopwatch.Stop();
        }
        /*
        public async void Run(IProgress<LoadingQueueState> progress, Action<TimeSpan> OnComplete)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AppendEndTask(); //this is a little dirty, but we stick in the end task just before you run.
            int totalStepCount = _queue.Count;
            long totalTicks = 0;
            IsRunning = true;
            while (_queue.Count > 0 && _totalSize > 0)
            {
                var next = _queue.Dequeue();
                Progress<float> taskProgress = new Progress<float>();
                taskProgress.ProgressChanged += (sender, taskProgressPercentage) =>
                {
                    next.SetTaskProgress(taskProgressPercentage);
                };
                var task = next.GetTask(taskProgress);
                await task;
                if (task.IsFaulted)
                {
                    foreach (var ex in task.Exception?.InnerExceptions ?? new(Array.Empty<Exception>()))
                    {
                        throw ex;
                    }
                }
                _completed += next.Size;
                float percentage = Math.Clamp((1.0f / _totalSize) * _completed, 0f, 1f);
                int currentStep = totalStepCount - _queue.Count;
                progress.Report(new LoadingQueueState()
                {
                    Percentage = percentage,
                    Name = next.Name,
                    Current = currentStep,
                    Total = totalStepCount,
                    Ticks = stopwatch.ElapsedTicks
                });
                totalTicks += stopwatch.ElapsedTicks;
                stopwatch.Restart();
            }
            await Task.Run(() => IsRunning = false);
            stopwatch.Stop();
            OnComplete?.Invoke(new TimeSpan(totalTicks));
        }*/
        
        private void AppendEndTask()
        {
            LoadingStep endTask = new Wait(1, "End", new TimeSpan(0,0,0, 300));
            Add(endTask);
        }
    }
}