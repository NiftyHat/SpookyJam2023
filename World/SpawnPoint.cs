using Godot;

namespace SpookyBotanyGame.World
{
    public partial class SpawnPoint : Node2D
    {
        public delegate void SpawnCompleted(SpawnPoint spawnPoint);
        [Export] 
        protected PackedScene _spawnedObject { get; set; }

        public void Spawn(Node2D node, SpawnCompleted onComplete)
        {
            GD.Print("Spawn");
            node.Position = this.Position;
            onComplete?.Invoke(this);
        }
    }
}