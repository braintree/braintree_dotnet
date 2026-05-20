using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Input fields for payer information.
    /// </summary>
    public class PayerInfoInput
    {
        public virtual BillingAddressInput BillingAddress { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string GivenName { get; protected set; }
        public virtual string PhoneCountryCode { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual ShippingAddressInput ShippingAddress { get; protected set; }
        public virtual string Surname { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();

            if (GivenName != null)
            {
                variables["givenName"] = GivenName;
            }
            if (Surname != null)
            {
                variables["surname"] = Surname;
            }
            if (Email != null)
            {
                variables["email"] = Email;
            }
            if (PhoneCountryCode != null)
            {
                variables["phoneCountryCode"] = PhoneCountryCode;
            }
            if (PhoneNumber != null)
            {
                variables["phoneNumber"] = PhoneNumber;
            }
            if (BillingAddress != null)
            {
                variables["billingAddress"] = BillingAddress.ToGraphQLVariables();
            }
            if (ShippingAddress != null)
            {
                variables["shippingAddress"] = ShippingAddress.ToGraphQLVariables();
            }

            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="PayerInfoInput"/>.
        /// </summary>
        /// <returns>A <see cref="PayerInfoInputBuilder"/> instance.</returns>
        public static PayerInfoInputBuilder Builder()
        {
            return new PayerInfoInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="PayerInfoInput"/>.
        /// </summary>
        public class PayerInfoInputBuilder
        {
            private PayerInfoInput payerInfoInput = new PayerInfoInput();

            /// <summary>
            /// Sets the billing address.
            /// </summary>
            /// <param name="billingAddress">The billing address.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder BillingAddress(BillingAddressInput billingAddress)
            {
                payerInfoInput.BillingAddress = billingAddress;
                return this;
            }

            /// <summary>
            /// Sets the email address.
            /// </summary>
            /// <param name="email">The email address.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder Email(string email)
            {
                payerInfoInput.Email = email;
                return this;
            }

            /// <summary>
            /// Sets the given name.
            /// </summary>
            /// <param name="givenName">The given name.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder GivenName(string givenName)
            {
                payerInfoInput.GivenName = givenName;
                return this;
            }

            /// <summary>
            /// Sets the phone country code.
            /// </summary>
            /// <param name="phoneCountryCode">The phone country code.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder PhoneCountryCode(string phoneCountryCode)
            {
                payerInfoInput.PhoneCountryCode = phoneCountryCode;
                return this;
            }

            /// <summary>
            /// Sets the phone number.
            /// </summary>
            /// <param name="phoneNumber">The phone number.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder PhoneNumber(string phoneNumber)
            {
                payerInfoInput.PhoneNumber = phoneNumber;
                return this;
            }

            /// <summary>
            /// Sets the shipping address.
            /// </summary>
            /// <param name="shippingAddress">The shipping address.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder ShippingAddress(ShippingAddressInput shippingAddress)
            {
                payerInfoInput.ShippingAddress = shippingAddress;
                return this;
            }

            /// <summary>
            /// Sets the surname.
            /// </summary>
            /// <param name="surname">The surname.</param>
            /// <returns>The builder instance.</returns>
            public PayerInfoInputBuilder Surname(string surname)
            {
                payerInfoInput.Surname = surname;
                return this;
            }

            public PayerInfoInput Build()
            {
                return payerInfoInput;
            }
        }
    }
}
