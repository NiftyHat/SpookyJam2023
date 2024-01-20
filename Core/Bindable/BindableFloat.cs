namespace SpookyBotanyGame.Core.Bindable
{
    public class BindableFloat : BindableStruct<float>
    {
        public BindableFloat() : base()
        {
        }

        public BindableFloat(float defaultValue)
        {
            _value = defaultValue;
        }
    }
}