using System;
using System.Threading.Tasks;

namespace SpookyBotanyGame.Core.Loading.Steps;

public class Execute : LoadingStep
{
    private Action _callback;
    
    public Execute(int size, string name, Action callback) : base(size, name)
    {
        _callback = callback;
    }
    
    public override void Run(Action<LoadingStep> onComplete, IProgress<float> progress = null)
    {
        _callback?.Invoke();
        progress?.Report(1f);
        onComplete?.Invoke(this);
    }

    public override Task GetTask(IProgress<float> progress = null)
    {
        return Task.Run(_callback);
    }
}