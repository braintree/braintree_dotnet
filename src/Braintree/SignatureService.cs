using System;

namespace Braintree
{
    public class SignatureService
    {
      public Hasher Hasher { get; set; }
      public string Key { get; set; }

      public string Sign(string payload) {
        string signature = Hash(payload);
        return signature + "|" + payload;
      }

      private string Hash(string payload) {
        return Hasher.HmacHash(Key, payload);
      }
    }
}
