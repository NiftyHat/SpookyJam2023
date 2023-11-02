using Godot;

namespace SpookyBotanyGame.Collectable
{
    [GlobalClass]
    public partial class CollectableResource : Resource, ICollectableType, ICollectableViewData
    {
        [Export] public string FriendlyName { get; set; }
        [Export] public char IconGlyph { get; set; }
        [Export] public Texture2D IconTexture { get; set; }
        [Export] public Texture2D CarriedTexture { get; set; }
        
        public CollectableResource()
        {
        }

        public CollectableResource(string friendlyName, char glyph, Texture2D iconTexture)
        {
            FriendlyName = friendlyName;
            IconGlyph = glyph;
            IconTexture = iconTexture;
        }

        public bool SameType(ICollectableType other)
        {
            System.Type otherType = other.GetType();
            return otherType == GetType();
        }
    }
}