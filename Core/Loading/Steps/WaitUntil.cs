using System;
using System.Threading.Tasks;

namespace SpookyBotanyGame.Core.Loading.Steps;

public class WaitUntil : LoadingStep
{
    protected DateTime _waitUntilTime;
    protected TimeSpan _pollingRate = new TimeSpan(0,0,0,0,33);

    public WaitUntil(int size, string name, DateTime waitUntilTime) : base(size, name)
    {
        _waitUntilTime = waitUntilTime;
    }

    public override Task GetTask(IProgress<float> progress = null)
    {
        return WaitUntilDateTime(_waitUntilTime);
    }

    public async Task WaitUntilDateTime(DateTime waitUntil)
    {
        while (DateTime.Now < waitUntil)
        {
            await Task.Delay(_pollingRate);
        }
    }
}