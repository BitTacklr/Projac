namespace Projac.Tests
{
    public class Signal<TMetadata>
    {
        public Signal()
        {
            IsSet = false;
        }

        public void Set(TMetadata metadata)
        {
            IsSet = true;
            Metadata = metadata;
        }

        public bool IsSet { get; private set; }

        public TMetadata Metadata { get; private set; }
    }
}
