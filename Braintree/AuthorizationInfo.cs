using System;
using System.Collections.Generic;

namespace Braintree
{
    public class AuthorizationInfo
    {
        public String MerchantId { get; set; }
        public String PublicKey { get; set; }
        public String PrivateKey { get; set; }
        public String ClientApiUrl { get; set; }
        public String AuthUrl { get; set; }
        public AuthorizationInfoOptions Options { get; set; }

        public string generate()
        {
            verifyOptions();
            String dateString = DateTime.Now.ToUniversalTime().ToString("s");

            List<String> queryParams = new List<String>();
            queryParams.Add("public_key=" + PublicKey);
            queryParams.Add("created_at=" + dateString);

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
            String fingerprint = signatureService.Sign(payload);
            return String.Format(
                "{{\"fingerprint\": \"{0}\", \"client_api_url\": \"{1}\", \"auth_url\": \"{2}\"}}",
                fingerprint,
                ClientApiUrl,
                AuthUrl
            );
        }

        private void verifyOptions()
        {
            if (Options != null && Options.CustomerId == null) {
                List<String> invalidOptions = new List<String>{};

                if (Options.VerifyCard != null) {
                    invalidOptions.Add("VerifyCard");
                }
                if (Options.MakeDefault != null) {
                    invalidOptions.Add("MakeDefault");
                }
                if (Options.FailOnDuplicatePaymentMethod != null) {
                    invalidOptions.Add("FailOnDuplicatePaymentMethod");
                }

                if (invalidOptions.Count != 0) {
                    string message = "Following arguments are invalid without CustomerId: ";
                    foreach(string invalidOption in invalidOptions) {
                        message += " " + invalidOption;
                    }
                    throw new ArgumentException(message);
                }
            }
        }
    }
}
