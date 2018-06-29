namespace libciscocdp
{
    using System.Net;

    /// <summary>
    /// Represent an IP prefix consisting of an IP address and a prefix length
    /// </summary>
    public class IPPrefix
    {
        /// <summary>
        /// The network address
        /// </summary>
        public IPAddress Network { get; set; }

        /// <summary>
        /// The length of the prefix
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Parse an IP prefix from a string
        /// </summary>
        /// <param name="source">The string to parse</param>
        /// <returns>Either the prefix or null if it fails</returns>
        public static IPPrefix Parse(string source)
        {
            var parts = source.Split("/");
            if (parts.Length != 2)
                return null;

            if (!IPAddress.TryParse(parts[0], out IPAddress network))
                return null;

            if (!int.TryParse(parts[1], out int length))
                return null;

            return new IPPrefix
            {
                Network = network,
                Length = length
            };
        }

        /// <summary>
        /// Compare another object to this object
        /// </summary>
        /// <param name="obj">The other object to compare to</param>
        /// <returns>true when equal</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (!(obj is IPPrefix))
                return false;

            if (obj == null && this == null)
                return true;

            if (obj == null || this == null)
                return false;

            return (obj as IPPrefix).Network.Equals(Network) &&
                (obj as IPPrefix).Length == Length;
        }

        /// <summary>
        /// Hash code generator
        /// </summary>
        /// <returns>A hashcode unique to this instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Convert the prefix to a string
        /// </summary>
        /// <returns>A string representation of the prefix</returns>
        public override string ToString()
        {
            return Network.ToString() + "/" + Length.ToString();
        }
    }
}
