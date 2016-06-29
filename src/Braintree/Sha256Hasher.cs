#pragma warning disable 1591

using System;
using System.Security.Cryptography;
using System.Text;

namespace Braintree
{
    public class Sha256Hasher : Hasher
    {
        public virtual string HmacHash(string key, string message)
        {
            var hmac = new HMACSHA256(Sha256Bytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public virtual byte[] Sha256Bytes(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            return SHA256.Create().ComputeHash(data);
        }
    }
}
