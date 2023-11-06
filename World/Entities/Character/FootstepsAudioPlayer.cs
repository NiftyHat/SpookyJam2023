using Godot;

namespace SpookyBotanyGame.World.Entities.Character;

public partial class FootstepsAudioPlayer : AudioStreamPlayer2D
{
    [Export] private AudioStream _left;
    [Export] private AudioStream _right;
}