using System;
using System.Threading.Tasks;

namespace SpookyBotanyGame.Core.Loading.Steps;

public class Wait : LoadingStep
{
    protected TimeSpan _timeSpan;
    
    public Wait(int size, string name, TimeSpan timeSpan) : base(size, name)
    {
        _timeSpan = timeSpan;
    }

    public override Task GetTask(IProgress<float> progress = null)
    {
        return Task.Delay(_timeSpan);
    }
}