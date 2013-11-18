#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Braintree
{
    public class Crypto
    {
        public virtual Boolean SecureCompare(String left, String right)
        {
            if (left == null || right == null || (left.Length != right.Length)) {
                return false;
            }

            byte[] leftBytes = Encoding.ASCII.GetBytes(left);
            byte[] rightBytes = Encoding.ASCII.GetBytes(right);

            int result = 0;
            for (int i=0; i < leftBytes.Length; i++) {
                result |= leftBytes[i] ^ rightBytes[i];
            }

            return result == 0;
        }

        public virtual String HmacHash(String key, String message)
        {
            var hmac = new HMACSHA1(Sha1Bytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public virtual byte[] Sha1Bytes(String s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            return SHA1.Create().ComputeHash(data);
        }

        public virtual String HmacHashSha256(String key, String message)
        {
            var hmac = new HMACSHA256(Sha256Bytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public virtual byte[] Sha256Bytes(String s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            return SHA256.Create().ComputeHash(data);
        }
    }
}
