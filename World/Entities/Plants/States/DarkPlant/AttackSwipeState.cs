using System;
using Godot;
using SpookyBotanyGame.Core.StateMachines;

namespace SpookyBotanyGame.World.Entities.Plants.States.DarkPlant
{
    public class AttackSwipeState : AttackState, IUpdatableState
    {
        private Node2D _target;
        private DarkPlantTentacle _tentacle;

        public override string AnimStart => "Swipe-Start";
        public override string AnimLoop => "Swipe-Loop";
        public override string AnimEnd => "Swipe-End";

        public AttackSwipeState(DarkPlantTentacle tentacle, Node2D target)
        {
            _tentacle = tentacle;
            _target = target;
            _tentacle.OnAnimationFinished += HandleAnimationComplete;
            DoSwipe();
        }

        private void DoSwipe()
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
            return target.GlobalPosition.Y > plant.GlobalPosition.Y && Math.Abs(target.GlobalPosition.Y - plant.GlobalPosition.Y) < 100;
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