using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// The details for the merchant who receives the funds and fulfills the order. The merchant is also known as the payee.
    /// </summary>
    public class PayPalPayeeInput
    {
        public virtual string EmailAddress { get; protected set; }
        public virtual string ClientId { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            if (EmailAddress != null)
            {
                variables["emailAddress"] = EmailAddress;
            }
            if (ClientId != null)
            {
                variables["clientId"] = ClientId;
            }
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="PayPalPayeeInput"/>.
        /// </summary>
        /// <returns>A <see cref="PayPalPayeeInputBuilder"/> instance.</returns>
        public static PayPalPayeeInputBuilder Builder()
        {
            return new PayPalPayeeInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="PayPalPayeeInput"/>.
        /// </summary>
        public class PayPalPayeeInputBuilder
        {
            private PayPalPayeeInput PayPalPayeeInput = new PayPalPayeeInput();

            /// <summary>
            /// Sets the email address of this merchant.
            /// </summary>
            /// <param name="emailAddress">The email address.</param>
            /// <returns>The builder instance.</returns>
            public PayPalPayeeInputBuilder EmailAddress(string emailAddress)
            {
                PayPalPayeeInput.EmailAddress = emailAddress;
                return this;
            }

            /// <summary>
            /// Sets the public ID for the payee- or merchant-created app. Introduced to support use cases, such as BrainTree integration with PayPal, where payee 'emailAddress' or 'merchantId' is not available.
            /// </summary>
            /// <param name="clientId">The public ID for the payee.</param>
            /// <returns>The builder instance.</returns>
            public PayPalPayeeInputBuilder ClientId(string clientId)
            {
                PayPalPayeeInput.ClientId = clientId;
                return this;
            }

            public PayPalPayeeInput Build()
            {
                return PayPalPayeeInput;
            }
        }
    }
}
