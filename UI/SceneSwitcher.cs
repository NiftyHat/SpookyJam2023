using Godot;

namespace SpookyBotanyGame.UI;

public partial class SceneSwitcher : Node
{
    public Node CurrentScene { get; set; }
    protected CanvasLayer _uiRoot;
    
    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }
    
    public void GoToScene(string path)
    {
        void DoSwitch()
        {
            CurrentScene.Free();
            var nextScene = (PackedScene)GD.Load(path);

            if (nextScene != null)
            {
                CurrentScene = nextScene.Instantiate();
                GetTree().Root.AddChild(CurrentScene);
                GetTree().CurrentScene = CurrentScene;
            }
            else
            {
                GD.PushError($"Couldn't load packed scene with path '{path}'");
            }
            GetTree().ProcessFrame -= DoSwitch;
        }
        GetTree().ProcessFrame += DoSwitch;
    }

    public void GoToScene(PackedScene packedScene)
    {
        void DoSwitch()
        {
            CurrentScene.Free();
            // Instance the new scene.
            CurrentScene = packedScene.Instantiate();

            // Add it to the active scene, as child of root.
            GetTree().Root.AddChild(CurrentScene);

            // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
            GetTree().CurrentScene = CurrentScene;
            GetTree().ProcessFrame -= DoSwitch;
        }
        GetTree().ProcessFrame += DoSwitch;
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
}