using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <summary>
    /// Customer identifying information for a PayPal customer session.
    /// </summary>
    public class CustomerSessionInput
    {
        public string Email { get; protected set; }
        public PhoneInput Phone { get; set; }
        public string DeviceFingerprintId { get; protected set; }
        public bool PaypalAppInstalled { get; protected set; }
        public bool VenmoAppInstalled { get; protected set; }
        public string UserAgent { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();

            if (Email != null)
            {
                variables.Add("email", Email);
            }
            if (Phone != null)
            {
                variables.Add("phone", Phone.ToGraphQLVariables());
            }
            if (DeviceFingerprintId != null)
            {
                variables.Add("deviceFingerprintId", DeviceFingerprintId);
            }
            variables.Add("paypalAppInstalled", PaypalAppInstalled);
            variables.Add("venmoAppInstalled", VenmoAppInstalled);
            if (UserAgent != null)
            {
                variables.Add("userAgent", UserAgent);
            }
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="CustomerSessionInput"/>.
        /// </summary>
        /// <returns>A <see cref="CustomerSessionInputBuilder"/> instance.</returns>
        public static CustomerSessionInputBuilder Builder()
        {
            return new CustomerSessionInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CustomerSessionInput"/>.
        /// </summary>
        public class CustomerSessionInputBuilder
        {
            private CustomerSessionInput customerSessionInput;

            public CustomerSessionInputBuilder()
            {
                customerSessionInput = new CustomerSessionInput();
            }

            /// <summary>
            /// Sets the customer email address.
            /// </summary>
            /// <param name="email">The customer email address.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder Email(string email)
            {
                customerSessionInput.Email = email;
                return this;
            }

            /// <summary>
            /// Sets the customer phone number input object.
            /// </summary>
            /// <param name="phone">The input object representing the customer phone number.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder Phone(PhoneInput phone)
            {
                customerSessionInput.Phone = phone;
                return this;
            }

            /// <summary>
            /// Sets the customer device fingerprint ID.
            /// </summary>
            /// <param name="deviceFingerprintId">The customer device fingerprint ID.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder DeviceFingerprintId(string deviceFingerprintId)
            {
                customerSessionInput.DeviceFingerprintId = deviceFingerprintId;
                return this;
            }

            /// <summary>
            /// Sets whether the PayPal app is installed on the customer's device.
            /// </summary>
            /// <param name="paypalAppInstalled">True if the PayPal app is installed, false otherwise.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder PaypalAppInstalled(bool paypalAppInstalled)
            {
                customerSessionInput.PaypalAppInstalled = paypalAppInstalled;
                return this;
            }

            /// <summary>
            /// Sets whether the Venmo app is installed on the customer's device.
            /// </summary>
            /// <param name="venmoAppInstalled">True if the Venmo app is installed, false otherwise.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder VenmoAppInstalled(bool venmoAppInstalled)
            {
                customerSessionInput.VenmoAppInstalled = venmoAppInstalled;
                return this;
            }

            /// <summary>
            /// Sets the user agent from the request originating from the customer's device.
            /// This will be used to identify the customer's operating system and browser versions.
            /// </summary>
            /// <param name="userAgent">The user agent.</param>
            /// <returns>The builder instance.</returns>
            public CustomerSessionInputBuilder UserAgent(string userAgent)
            {
                customerSessionInput.UserAgent = userAgent;
                return this;
            }

            public CustomerSessionInput Build()
            {
                return customerSessionInput;
            }
        }
    }
}
