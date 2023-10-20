using Godot;

namespace SpookyBotanyGame.Collectable
{
    public interface ICollectableViewData
    {
        string FriendlyName { get; }
        char IconGlyph { get;  }
        Texture2D IconTexture { get;}
    }
}

