using Godot;

namespace SpookyBotanyGame.UI.Widgets
{
    [GlobalClass]
    public partial class InputUITweenAnimator : Node
    {
        [Export] public InputUIAnimationViewData ViewData { get; set; }

        [Export] public TextureRect TextureRect { get; set; }

        [Export] public Control KeyLabelNode { get; set; }

        private Tween _tweenPressed;
        private Texture2D _idleTexture;
        private Texture2D[] _pressedFrames;
        private Vector2 _labelOrigin;
        private Callable _setIdleState;

        public override void _Ready()
        {
            base._Ready();
            _labelOrigin = KeyLabelNode.Position;
            Set(ViewData);
        }

        public void Set(InputUIAnimationViewData viewData)
        {
            if (viewData == null)
            {
                GD.PushError($"Assign null viewData to {this.Name}");
                return;
            }

            ViewData = viewData;
            _pressedFrames = ViewData.PressedFrames;
            _idleTexture = ViewData.IdleTexture;
            if (TextureRect != null)
            {
                TextureRect.Texture = _idleTexture;
            }
        }

        public void Pressed()
        {
            _tweenPressed?.Kill();
            _tweenPressed = GetTweenPressed(_pressedFrames);
        }

        public Tween GetTweenPressed(Texture2D[] frameList)
        {
            Tween tween = GetTree().CreateTween();
            int frameCount = frameList.Length;
            for (int i = 0; i < frameCount; i++)
            {
                var texture = frameList[i];
                int frame = i;
                tween.TweenCallback(Callable.From(() => SetAnimationState(texture, frame))).SetDelay(0.1f);
            }
            tween.TweenCallback(Callable.From(SetIdleState));
            return tween;
        }

        private void SetIdleState()
        {
            TextureRect.Texture = _idleTexture;
            if (KeyLabelNode != null)
            {
                KeyLabelNode.Position = _labelOrigin;
            }
        }

        private void SetAnimationState(Texture2D texture, int frame)
        {
            TextureRect.Texture = texture;
            if (KeyLabelNode != null)
            {
                switch (frame)
                {
                    case 1:
                    case 2:
                    case 3:
                        KeyLabelNode.Position = _labelOrigin + Vector2.Down * frame * 2;
                        break;
                    default:
                        KeyLabelNode.Position = _labelOrigin;
                        break;
                }
            }
        }
    }
}