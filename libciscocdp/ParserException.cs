namespace libciscocdp
{
    using System;

    public class ParserException : Exception
    {
        public byte[] Buffer { get; }
        public int Index { get; }

        public ParserException(byte[] buffer, int index, string message) : base(message)
        {
            Buffer = buffer;
            Index = index;
        }
    }
}
