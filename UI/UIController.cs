using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using SpookyBotanyGame.UI.Screens.Pause;
using SpookyBotanyGame.UI.Screens.Settings;

namespace SpookyBotanyGame.UI;

public partial class UIController : Node
{
    public Node CurrentScene { get; set; }
    public UIData Data { get; set; }
    
    protected UISceneData _sceneData;

    protected HashSet<Node> _activeScenes = new HashSet<Node>();
    protected SceneFactory _sceneFactory;
    protected UIRoot _uiRoot;
    protected SceneSwitcher _sceneSwitcher;
    protected PackedScene _uiRootAsset;
    
    public override void _Ready()
    {
        _sceneSwitcher = GetNode<SceneSwitcher>("/root/SceneSwitcher");
        if (_sceneSwitcher == null)
        {
            GD.PushError($"Missing AutoLoad Node SceneSwitcher");
        }
        const string uiDataPath = "res://UI/UIData.tres";
        Data = (UIData)GD.Load(uiDataPath);
        if (Data == null)
        {
            GD.PushError($"Missing resource '{uiDataPath}'");
            return;
        }
        _sceneData = Data.Scenes;
        _sceneFactory = new SceneFactory();
        _sceneFactory.Add<SettingsScreen>(_sceneData.Settings);
        _sceneFactory.Add<PauseScreen>(_sceneData.Pause);
        _uiRoot = GetNodeOrNull<UIRoot>("/root/UIRoot");
        if (_uiRoot == null)
        {
            _uiRoot = Data.UIRoot.Instantiate<UIRoot>();
        }
    }

    public void ApplicationHome()
    {
        if (_sceneData.Title != null)
        {
            _sceneSwitcher.GoToScene(_sceneData.Title);
        }
    }

    public void ApplicationPlay()
    {
        if (_sceneData.Game != null)
        {
            _sceneSwitcher.GoToScene(_sceneData.Game);
        }
    }
    
    public bool Close<TScreenClass>() where TScreenClass : Node
    {
        if (TryGetExising(out TScreenClass instance))
        {
            _uiRoot.RemoveChild(instance);
            return true;
        }
        return false;
    }

    public TScreenClass Open<TScreenClass>(PackedScene packedScene) where TScreenClass : Node
    {
        if (_uiRoot.GetParent() == null)
        {
            Viewport root = GetTree().Root;
            root.AddChild(_uiRoot);
        }
        TScreenClass instance = null;
        if (TryGetExising(out instance))
        {
            return instance;
        }
        instance = packedScene.Instantiate<TScreenClass>();
        return instance;
    }

    public bool TryGetExising<TScreenClass>(out TScreenClass instance) where TScreenClass : Node
    {
        System.Type existingType = typeof(TScreenClass);
        Node existingScene = _activeScenes.FirstOrDefault(item => item.GetType() == existingType);
        if (existingScene is TScreenClass typedScene)
        {
            if (existingScene.GetParent() != _uiRoot)
            {
                _uiRoot.AddChild(existingScene);
            }
            instance = typedScene;
            return true;
        }
        instance = null;
        return false;
    }

    public TScreenClass Open<TScreenClass>() where TScreenClass : Node
    {
        if (_uiRoot.GetParent() == null)
        {
            Viewport root = GetTree().Root;
            root.AddChild(_uiRoot);
        }
        TScreenClass instance;
        if (TryGetExising(out instance))
        {
            return instance;
        }
        instance = _sceneFactory.Instantiate<TScreenClass>();
        if (instance != null)
        {
            instance.TreeExiting += () =>
            {
                _activeScenes.Remove(instance);
                GD.Print(PrintActiveScenes());
            };
            _uiRoot.AddChild(instance);
            //_uiRoot.MoveChild(instance);
        }
        return instance;
    }

    public Node Peek()
    {
        var children = _uiRoot.GetChildren(false);
        return children[^1];
    }

    private string PrintActiveScenes()
    {
        StringBuilder sb = new StringBuilder("Active Screens :\n");
        foreach (var item in _activeScenes)
        {
            sb.Append(item.Name);
            sb.Append('\n');
        }

        if (sb.Length > 0)
        {
            sb.Length -= 1;
        }
        return sb.ToString();
    }

    public void ApplicationQuit()
    {
        _sceneSwitcher.ApplicationQuit();
    }
}