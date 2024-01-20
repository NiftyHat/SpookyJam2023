using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using SpookyBotanyGame.Core.Loading;
using SpookyBotanyGame.Core.Loading.Steps;

namespace SpookyBotanyGame.Core;

[GlobalClass]
public partial class BootScreen : Control
{
    [Export] public RichTextLabel Log { get; set; }
    [Export] public ProgressBar LoadingBar { get; set; }
    //[Export] public PackedScene LoadCompleteScene { get; set; }
    [Export(PropertyHint.File, "*.tscn")] private string PathSceneLoadOnComplete { get; set; }

    protected StringBuilder _log = new StringBuilder();
    protected LoadingQueue _loadingQueue;
    protected Array<Node> _autoLoadNodes;
    
    protected float _loadingPercentage = 0f;
    protected System.Collections.Generic.Queue<string> _logMessageQueue = new Queue<string>();
    
    public void Print(string text)
    {
        _log.AppendLine(text);
        Log.AppendText(text);
        Log.Newline();
    }
    
    public void PrintError(string text)
    {
        _log.AppendLine($"ERROR:{text}");
        string errorFormattedText = $"[color=red] {text} [/color]";
        Log.AppendText(errorFormattedText);
        Log.Newline();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Print("Boot Scene Enter Tree()");
    }

    public override void _Ready()
    {
        base._Ready();
        Print("Bootscene Ready");
        _autoLoadNodes = GetAutoLoadNodes();
        _loadingQueue = new LoadingQueue();
        _loadingQueue.Add(new Execute(1, "Checking Subsystems", () =>
        {
            var callable = Callable.From<Array<Node>>(LogNodeNames);
            callable.CallDeferred(_autoLoadNodes);
        }));
        _loadingQueue.Add(new LoadResource<PackedScene>(5, $"Loading Title Scene {PathSceneLoadOnComplete}", PathSceneLoadOnComplete));
        Wait(5.0, StartLoadQueue);
    }

    private void StartLoadQueue()
    {
        _logMessageQueue.Enqueue("StartLoadQueue Ready");
        Progress<LoadingQueueState> progress = new Progress<LoadingQueueState>();
        progress.ProgressChanged += OnLoadingProgressChange;
        _loadingQueue.Run(progress, HandleLoadQueueComplete);
    }

    private void HandleLoadQueueComplete(TimeSpan bootTime)
    {
        _logMessageQueue.Enqueue($"Done after {FormatBootTime(bootTime)}");
    }

    private string FormatBootTime(TimeSpan ts)
    {
        if (ts.TotalMilliseconds <= 0)
        {
            return $"{ts.Ticks} ticks";
        }
        if (ts.TotalSeconds <= 0)
        {
            return $"{ts.TotalMilliseconds:N0}ms";
        }
        if (ts.TotalMinutes <= 0)
        {
            return $"{ts.TotalSeconds:N0}s {ts.Milliseconds:N0}ms";
        }
        return $"{ts.TotalMinutes:N0}m {ts.Seconds:N0}s {ts.Milliseconds:N0}ms";
    }

    private void LogNodeNames(Array<Node> nodeList)
    {
        foreach (var item in nodeList)
        {
            Print(item.Name);
        }
    }

    private void OnLoadingProgressChange(object sender, LoadingQueueState loadingState)
    {
        _loadingPercentage = loadingState.Percentage;
        string logMessage =
            $"Step [{loadingState.Current}/{loadingState.Total}] {loadingState.Name} {loadingState.Percentage * 100:N1}%";
        _logMessageQueue.Enqueue(logMessage);
        //LoadingBar.Value = loadingState.Percentage;
        //Print();
    }

    public override void _Process(double delta)
    {
        LoadingBar.Value = _loadingPercentage;
        if (_logMessageQueue.Count > 0)
        {
            string msg = _logMessageQueue.Dequeue();
            Print(msg);
        }
        base._Process(delta);
    }

    private Array<Node> GetAutoLoadNodes()
    {
        var rootChildren = GetTree().Root.GetChildren();
        if (rootChildren.Contains(this))
        {
            rootChildren.Remove(this);
        }
        return rootChildren;
    }

    /*
    public async Task<bool> BootAsync()
    {
        Print("Started BootAsync");
        var timeStart = Time.GetTicksMsec();
        LoadingBar.Value = 0.1f;
        var rootChildren = GetTree().Root.GetChildren();
        foreach (var item in rootChildren)
        {
            if (item != null)
            {
                Print($"Validated Autoload System {item.Name}");
            }
        }

        if (LoadCompleteScene == null)
        {
            PrintError("No scene to load. Set a load scene in boot.tscn");
            return false;
        }
        var now = Time.GetTicksMsec();
        TimeSpan elapsed = new TimeSpan((long)now - (long)timeStart);
        TimeSpan waitTime = _minDisplayTime - elapsed;
        await Wait(waitTime.TotalSeconds);
        return true;
    }*/
    
    public void GoToScene(PackedScene packedScene)
    {
        void DoSwitch()
        {
            Viewport root = GetTree().Root;
            var currentScene = root.GetChild(root.GetChildCount() - 1);
            currentScene.Free();
            // Instance the new scene.
            var nextScene = packedScene.Instantiate();

            // Add it to the active scene, as child of root.
            GetTree().Root.AddChild(nextScene);

            // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
            GetTree().CurrentScene = nextScene;
            GetTree().ProcessFrame -= DoSwitch;
        }
        GetTree().ProcessFrame += DoSwitch;
    }

    public async Task Wait(double seconds)
    {
        var timer = GetTree().CreateTimer(seconds);
        await ToSignal(timer, "timeout");
    }
    async void Wait(double seconds, System.Action callback)
    {
        if (seconds > 0)
        {
            GetTree().CreateTimer(seconds);
            await ToSignal(GetTree().CreateTimer(seconds), "timeout");
            await Task.Run(callback);
        }
        else
        {
            callback();
        }
    }
}