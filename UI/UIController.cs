using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.UI;

public partial class UIController : Node
{
    public Node CurrentScene { get; set; }
    public UIData Data { get; set; }
    
    public StateMachine StateMachine { get; set; }

    protected UISceneData _sceneData;
    

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        StateMachine = new StateMachine("UIState", true);
        string uiDataPath = "res://UI/UIData.tres";
        Data = (UIData)GD.Load(uiDataPath);
        if (Data == null)
        {
            GD.PushError("Missing resource '{uiDataPath}'");
            return;
        }
        _sceneData = Data.Scenes;
    }
    
    public void GotoScene(string path)
    {
        void DoSwitch()
        {
            SwitchScene(path);
            GetTree().ProcessFrame -= DoSwitch;
        }
        GetTree().ProcessFrame += DoSwitch;
        
    }

    private void GoToScene(PackedScene packedScene)
    {
        void DoSwitch()
        {
            SwitchScene(packedScene);
            GetTree().ProcessFrame -= DoSwitch;
        }
        GetTree().ProcessFrame += DoSwitch;
    }

    private void SwitchScene(PackedScene packedScene)
    {
        // Instance the new scene.
        CurrentScene = packedScene.Instantiate();

        // Add it to the active scene, as child of root.
        GetTree().Root.AddChild(CurrentScene);

        // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
        GetTree().CurrentScene = CurrentScene;
    }
    
    private void SwitchScene(string path)
    {
        CurrentScene.Free();
        var nextScene = (PackedScene)GD.Load(path);

        if (nextScene != null)
        {
            SwitchScene(nextScene);
        }
        else
        {
            GD.PushError($"Couldn't load packed scene with path '{path}'");
        }
    }

    public void ApplicationQuit()
    {
        var sceneTree = GetTree();
        if (sceneTree != null)
        {
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
            GetTree().Quit();
        }
    }

    public void ApplicationHome()
    {
        if (_sceneData.Title != null)
        {
            SwitchScene(_sceneData.Title);
        }
    }

    public void ApplicationPlay()
    {
        if (_sceneData.Game != null)
        {
            SwitchScene(_sceneData.Game);
        }
    }
}