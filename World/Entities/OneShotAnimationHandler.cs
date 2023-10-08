using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.World.Entities
{
    /// <summary>
    /// Generic One Shot animation callback registry for animation player.
    /// </summary>
    public class OneShotAnimationHandler
    {
        private Dictionary<string, List<AnimationPlayer.AnimationFinishedEventHandler>> _items;
        private bool _isEmpty;

        public OneShotAnimationHandler()
        {
            _items = new Dictionary<string, List<AnimationPlayer.AnimationFinishedEventHandler>>();
        }

        private AnimationPlayer _animation;
        public AnimationPlayer Animation
        {
            get => _animation;
            set => SetAnimationPlayer(value);
        }

        private void SetAnimationPlayer(AnimationPlayer animation)
        {
            //if there is an existing animation clear out the existing queued events.
            if (_animation != null)
            {
                if (!_isEmpty)
                {
                    Animation.AnimationFinished -= HandleAnimationFinished;
                }
                _items.Clear();
                _isEmpty = true;
            }
            _animation = animation;
        }

        public void Add(string animationName, AnimationPlayer.AnimationFinishedEventHandler handler)
        {
            var animation = Animation.GetAnimation(animationName);
            if (animation == null)
            {
                GD.PushWarning($"{nameof(OneShotAnimationHandler)} {nameof(Add)}() animation '{animationName}' doesn't exist {Animation.Name}");
                return;
            }
            if (animation.LoopMode != Godot.Animation.LoopModeEnum.None)
            {
                GD.PushWarning($"{nameof(OneShotAnimationHandler)} {nameof(Add)}() animation '{animationName}' uses loopmode {animation.LoopMode.ToString()} and will never dispatch");
                return;
            }
            if (_isEmpty)
            {
                _isEmpty = false;
                Animation.AnimationFinished += HandleAnimationFinished;
            }
            if ( _items.TryGetValue(animationName, out var list))
            {
                list.Add(handler);
            }
            else
            {
                _items[animationName] = new List<AnimationPlayer.AnimationFinishedEventHandler>() {handler};
            }
        }
        
        private void Remove(StringName animationName)
        {
            _items.Remove(animationName);
            if (_items.Keys.Count == 0)
            {
                _isEmpty = true;
                Animation.AnimationFinished -= HandleAnimationFinished;
            }
        }
        
        private void HandleAnimationFinished(StringName animationName)
        {
            if (_items.TryGetValue(animationName, out var list))
            {
                foreach (var item in list)
                {
                    item.Invoke(animationName);
                }
                Remove(animationName);
            }
        }


    }
}