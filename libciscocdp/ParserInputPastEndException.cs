namespace libciscocdp
{
    public class ParserInputPastEndException : ParserException
    {
        public string Operation { get; }

        public ParserInputPastEndException(byte[] buffer, int index, string operation, string message) : base(buffer, index, message)
        {
            Operation = operation;
        }
    }
}
