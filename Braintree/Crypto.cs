using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Braintree
{
    public class Crypto
    {
        public String HmacHash(String key, String message)
        {
            var hmac = new HMACSHA1(Sha1Bytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public byte[] Sha1Bytes(String s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            return new SHA1CryptoServiceProvider().ComputeHash(data);
        }

    }
}
