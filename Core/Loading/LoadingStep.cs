using System;
using System.Threading.Tasks;

namespace SpookyBotanyGame.Core.Loading.Steps;

public abstract class LoadingStep
{
    public readonly int Size;
    public string Name;
    public float TaskProgress;
    public LoadingStep(int size, string name)
    {
        Size = size;
        Name = name;
    }

    public abstract void Run(Action<LoadingStep> onComplete, IProgress<float> progress = null);

    public abstract Task GetTask(IProgress<float> progress = null);

    public void SetTaskProgress(float taskProgress)
    {
        TaskProgress = taskProgress;
    }
}