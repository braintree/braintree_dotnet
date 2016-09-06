#pragma warning disable 1591

using System.Text;

namespace Braintree
{
    public class Crypto
    {
        public virtual bool SecureCompare(string left, string right)
        {
            if (left == null || right == null || (left.Length != right.Length))
            {
                return false;
            }

            byte[] leftBytes = Encoding.ASCII.GetBytes(left);
            byte[] rightBytes = Encoding.ASCII.GetBytes(right);

            int result = 0;
            for (int i=0; i < leftBytes.Length; i++)
            {
                result |= leftBytes[i] ^ rightBytes[i];
            }

            return result == 0;
        }
    }
}
