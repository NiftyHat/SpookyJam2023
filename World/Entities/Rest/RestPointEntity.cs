using Godot;
using SpookyBotanyGame.World.Entities.Properties;

namespace SpookyBotanyGame.World.Entities.Rest
{
    [GlobalClass]
    public partial class RestPointEntity : GameEntity
    {
        [Export] public Interactable Interaction { get; set; }
        [Export] public SimController Sim { get; set; }

        public override void _Ready()
        {
            base._Ready();
            if (Interaction != null)
            {
                Interaction.OnInteractionTriggered += HandleInteractionTriggered;
            }
        }

        private bool HandleInteractionTriggered(GameEntity other, GameEntity self)
        {
            GD.Print("HandleInteractionTriggered");
            if (self == this && other is PlayerEntity playerEntity)
            {
                Sim.PlayerSleep(playerEntity);
                return true;
            }
            return false;
        }
    }
}

