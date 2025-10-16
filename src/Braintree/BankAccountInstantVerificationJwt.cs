#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// Represents a Bank Account Instant Verification JWT containing a JWT token.
    /// </summary>
    public class BankAccountInstantVerificationJwt
    {
        /// <summary>
        /// Returns the JWT for Bank Account Instant Verification.
        /// </summary>
        public virtual string Jwt { get; protected set; }


        public BankAccountInstantVerificationJwt(string jwt)
        {
            Jwt = jwt;
        }
    }
}