using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkPlant
{
    public partial class AttackSideState : AttackState
    {
        private Node2D _target;
        private DarkPlantTentacle _tentacle;
        private Direction _direction;

        public override string AnimStart => "Pap-" + _direction.Name + "-Start";
        public override string AnimLoop =>  "Pap-" + _direction.Name + "-Loop";
        public override string AnimEnd =>  "Pap-" + _direction.Name + "-End";

        public AttackSideState(DarkPlantTentacle tentacle, Node2D target)
        {
            _tentacle = tentacle;
            _target = target;
            _tentacle.OnAnimationFinished += HandleAnimationComplete;
            if (target.GlobalPosition.X > _tentacle.GlobalPosition.X)
            {
                _direction = Direction.East;
            }
            else
            {
                _direction = Direction.West;
            }
            DoAttack();
        }

        private void DoAttack()
        {
            _tentacle.Animation.Play(AnimStart);
        }

        private void HandleAnimationComplete(string animName)
        {
            if (animName == AnimStart)
            {
                _tentacle.Animation.Play(AnimLoop);
                return;
            }
            if (animName == AnimLoop)
            {
                if (_tentacle.IsLightTriggeringAttack && CanTarget(_tentacle, _target))
                {
                    _tentacle.Animation.Play(AnimLoop);
                }
                else
                {
                    _tentacle.Animation.Play(AnimEnd);
                }
            }
            if (animName == AnimEnd)
            {
                Exit(new WaitForTargetState(_tentacle));
            }
        }

        public static bool CanTarget(DarkPlantTentacle plant, Node2D target)
        {
            return target.GlobalPosition.Y <= plant.GlobalPosition.Y;
        }

        protected override void Exit(State state = null)
        {
            _tentacle.OnAnimationFinished -= HandleAnimationComplete;
            base.Exit(state);
        }
        
        public void Process(double delta)
        {
            
        }
    }
}