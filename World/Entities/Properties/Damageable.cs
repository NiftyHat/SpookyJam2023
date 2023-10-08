using Godot;

namespace SpookyBotanyGame.World.Entities.Properties
{
    [GlobalClass]
    public partial class Damageable : EntityProperty
    {
        [Export] public int HealthMax { get; set; }
        private int _health;
        
        public void TakeDamage(int amount)
        {
        }
        
    }
}