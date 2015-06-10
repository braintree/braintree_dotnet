#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Braintree
{
    public interface Hasher
    {
      string HmacHash(string key, string content);
    }
}
