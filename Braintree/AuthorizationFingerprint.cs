using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class AuthorizationFingerprint
    {
        public String MerchantId { get; set; }
        public String PublicKey { get; set; }
        public String PrivateKey { get; set; }
        public AuthorizationFingerprintOptions Options { get; set; }

        public string generate()
        {
            String dateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            List<String> queryParams = new List<String>();
            queryParams.Add("public_key=" + PublicKey);
            queryParams.Add("created_at=" + dateString);
            queryParams.Add("merchant_id=" + MerchantId);

            if (Options != null && Options.CustomerId != null) {
              queryParams.Add("customer_id=" + Options.CustomerId);
            }
            if (Options != null && Options.MakeDefault != null) {
              queryParams.Add("credit_card[options][make_default]=" + Options.MakeDefault.ToString().ToLower());
            }
            if (Options != null && Options.FailOnDuplicatePaymentMethod != null) {
              queryParams.Add("credit_card[options][fail_on_duplicate_payment_method]=" + Options.FailOnDuplicatePaymentMethod.ToString().ToLower());
            }
            if (Options != null && Options.VerifyCard != null) {
              queryParams.Add("credit_card[options][verify_card]=" + Options.VerifyCard.ToString().ToLower());
            }

            String payload = String.Join("&", queryParams.ToArray());
            SignatureService signatureService = new SignatureService { Key = PrivateKey, Hasher = new Sha256Hasher() };
            return signatureService.Sign(payload);
        }
    }
}
