namespace SpookyBotanyGame.Core;

public interface IEnabled
{
    bool IsEnabled { get; }
    void SetEnabled(bool isEnabled);
}