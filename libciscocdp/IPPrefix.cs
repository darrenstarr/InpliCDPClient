namespace libciscocdp
{
    using System.Net;

    public class IPPrefix
    {
        public IPAddress Network { get; set; }
        public int Length { get; set; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null && this == null)
                return true;

            if (obj == null || this == null)
                return false;

            return (obj as IPPrefix).Network.Equals(Network) &&
                (obj as IPPrefix).Length == Length;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Network.ToString() + "/" + Length.ToString();
        }
    }
}
