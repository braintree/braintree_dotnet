using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <summary>
    /// Phone number input for PayPal customer session.
    /// </summary>
    public class PhoneInput
    {
        public virtual string CountryPhoneCode { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual string ExtensionNumber { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            if (CountryPhoneCode != null)
            {
                variables["countryPhoneCode"] = CountryPhoneCode;
            }
            if (PhoneNumber != null)
            {
                variables["phoneNumber"] = PhoneNumber;
            }
            if (ExtensionNumber != null)
            {
                variables["extensionNumber"] = ExtensionNumber;
            }
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="PhoneInput"/>.
        /// </summary>
        /// <returns>A <see cref="PhoneInputBuilder"/> instance.</returns>
        public static PhoneInputBuilder Builder()
        {
            return new PhoneInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="PhoneInput"/>.
        /// </summary>
        public class PhoneInputBuilder
        {
            private PhoneInput phoneInput = new PhoneInput();

            /// <summary>
            /// Sets the country phone code.
            /// </summary>
            /// <param name="countryPhoneCode">The country phone code.</param>
            /// <returns>The builder instance.</returns>
            public PhoneInputBuilder CountryPhoneCode(string countryPhoneCode)
            {
                phoneInput.CountryPhoneCode = countryPhoneCode;
                return this;
            }

            /// <summary>
            /// Sets the phone number.
            /// </summary>
            /// <param name="phoneNumber">The phone number.</param>
            /// <returns>The builder instance.</returns>
            public PhoneInputBuilder PhoneNumber(string phoneNumber)
            {
                phoneInput.PhoneNumber = phoneNumber;
                return this;
            }

            /// <summary>
            /// Sets the extension number.
            /// </summary>
            /// <param name="extensionNumber">The extension number.</param>
            /// <returns>The builder instance.</returns>
            public PhoneInputBuilder ExtensionNumber(string extensionNumber)
            {
                phoneInput.ExtensionNumber = extensionNumber;
                return this;
            }

            public PhoneInput Build()
            {
                return phoneInput;
            }
        }
    }
}
