#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Braintree
{
    public class Sha256Hasher : Hasher
    {
        public virtual String HmacHash(String key, String message)
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
