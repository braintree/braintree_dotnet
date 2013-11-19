using System;

namespace Braintree
{
    public class SignatureService
    {
      public Hasher Hasher { get; set; }
      public String Key { get; set; }

      public String Sign(String payload) {
        String signature = Hash(payload);
        return signature + "|" + payload;
      }

      private String Hash(String payload) {
        return Hasher.HmacHash(Key, payload);
      }
    }
}
