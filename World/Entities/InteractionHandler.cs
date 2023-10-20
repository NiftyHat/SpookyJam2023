using Godot;

namespace SpookyBotanyGame.World.Entities
{
    [GlobalClass]
    public partial class InteractionHandler : Node2D
    {
        [Export] public Sprite2D Sprite { get; set; }
        [Export] public ShaderMaterial InteractionMaterial { get; set; }

        private Material _interactionMaterial;
        private Material _defaultMaterial;

        public override void _Ready()
        {
            _defaultMaterial = Sprite.Material;
            base._Ready();
        }

        public void OutlineShaderShow()
        {
            if (_interactionMaterial == null)
            {
                _interactionMaterial = InteractionMaterial.Duplicate() as ShaderMaterial;
            }
            Sprite.Material = InteractionMaterial;
        }

        public void OutlineShaderHide()
        {
            Sprite.Material = _defaultMaterial;
            _interactionMaterial = null;
        }
    }
}