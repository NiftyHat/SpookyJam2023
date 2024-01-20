using System;
using System.Collections.Generic;
using Godot;
using SpookyBotanyGame.UI.Screens.Settings;

namespace SpookyBotanyGame.UI
{
    [GlobalClass]
    public partial class UISceneData : Resource
    {
        [Export] public PackedScene Title { get; set; }
        [Export] public PackedScene GameOver { get; set; }
        [Export] public PackedScene Game { get; set; }
        [Export] public PackedScene Settings { get; set; }
        [Export] public PackedScene Pause { get; set; }
        
    }
}