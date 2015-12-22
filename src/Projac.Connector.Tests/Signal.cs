namespace Projac.Connector.Tests
{
    public class Signal
    {
        private bool _set;

        public Signal()
        {
            _set = false;
        }

        public void Set()
        {
            _set = true;
        }

        public bool IsSet { get { return _set; } }
    }
}
