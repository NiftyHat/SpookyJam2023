using Godot;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class SimController : Node
    {
        protected SimSystem _sim;

        public override void _Ready()
        {
            base._Ready();
            _sim = GetNode<SimSystem>("/root/SimSystem");
            if (_sim == null)
            {
                GD.PrintErr($"{nameof(SimAdvanceable)} didn't attach to SimSystem.Time");
            }
        }

        public void PlayerSleep(PlayerEntity playerEntity)
        {
            _sim.PlayerSleep(playerEntity);
        }

        public void PlayerRespawn(PlayerEntity playerEntity)
        {
            _sim.PlayerRespawn(playerEntity);
        }
    }
}

