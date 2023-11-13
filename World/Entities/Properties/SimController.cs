using Godot;
using SpookyBotanyGame.Core;
using SpookyBotanyGame.World.Systems;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass,Icon("res://World/Entities/icon-entity-property.svg")]
    public partial class SimController : Node
    {
        protected SimSystem _sim;

        public FadeEvents DayEndEvents => _sim.DayEndEvents;
        public FadeEvents DayStartEvents => _sim.DayStartEvents;

        public override void _Ready()
        {
            base._Ready();
            _sim = GetNode<SimSystem>("/root/SimSystem");
            if (_sim == null)
            {
                GD.PrintErr($"{nameof(SimAdvanceable)} didn't attach to SimSystem.Time");
            }
        }

        public void PlayerRest(PlayerEntity playerEntity)
        {
            _sim.PlayerRest(playerEntity);
        }

        public void PlayerRespawn(PlayerEntity playerEntity)
        {
            _sim.PlayerRespawn(playerEntity);
        }
    }
}

