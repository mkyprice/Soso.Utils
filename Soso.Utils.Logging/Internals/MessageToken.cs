namespace Soso.Utils.Logging.Internals
{
    public readonly struct MessageToken
    {
        public readonly int Index;
        public readonly int Length;
        public readonly int PropertyIndex;

        public MessageToken(int index, int length, int propertyIndex)
        {
            Index = index;
            Length = length;
            PropertyIndex = propertyIndex;
        }
    }
}