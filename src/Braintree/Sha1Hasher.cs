#pragma warning disable 1591

using System;
using System.Security.Cryptography;
using System.Text;

namespace Braintree
{
    public class Sha1Hasher : Hasher
    {
        public virtual string HmacHash(string key, string message)
        {
            var hmac = new HMACSHA1(Sha1Bytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public virtual byte[] Sha1Bytes(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            return SHA1.Create().ComputeHash(data);
        }
    }
}
