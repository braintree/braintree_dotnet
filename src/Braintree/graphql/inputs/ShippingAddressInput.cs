using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Input fields for shipping address information.
    /// </summary>
    public class ShippingAddressInput
    {
        public virtual string CountryCodeAlpha2 { get; protected set; }
        public virtual string ExtendedAddress { get; protected set; }
        public virtual string Locality { get; protected set; }
        public virtual string PostalCode { get; protected set; }
        public virtual string Region { get; protected set; }
        public virtual string StreetAddress { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();

            if (CountryCodeAlpha2 != null)
            {
                variables["countryCode"] = CountryCodeAlpha2;
            }
            if (StreetAddress != null)
            {
                variables["streetAddress"] = StreetAddress;
            }
            if (ExtendedAddress != null)
            {
                variables["extendedAddress"] = ExtendedAddress;
            }
            if (Locality != null)
            {
                variables["locality"] = Locality;
            }
            if (Region != null)
            {
                variables["region"] = Region;
            }
            if (PostalCode != null)
            {
                variables["postalCode"] = PostalCode;
            }

            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="ShippingAddressInput"/>.
        /// </summary>
        /// <returns>A <see cref="ShippingAddressInputBuilder"/> instance.</returns>
        public static ShippingAddressInputBuilder Builder()
        {
            return new ShippingAddressInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="ShippingAddressInput"/>.
        /// </summary>
        public class ShippingAddressInputBuilder
        {
            private ShippingAddressInput shippingAddressInput = new ShippingAddressInput();

            /// <summary>
            /// Sets the country code.
            /// </summary>
            /// <param name="countryCodeAlpha2">The country code.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder CountryCode(string countryCodeAlpha2)
            {
                shippingAddressInput.CountryCodeAlpha2 = countryCodeAlpha2;
                return this;
            }

            /// <summary>
            /// Sets the extended address.
            /// </summary>
            /// <param name="extendedAddress">The extended address.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder ExtendedAddress(string extendedAddress)
            {
                shippingAddressInput.ExtendedAddress = extendedAddress;
                return this;
            }

            /// <summary>
            /// Sets the city/locality.
            /// </summary>
            /// <param name="locality">The city/locality.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder Locality(string locality)
            {
                shippingAddressInput.Locality = locality;
                return this;
            }

            /// <summary>
            /// Sets the postal code.
            /// </summary>
            /// <param name="postalCode">The postal code.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder PostalCode(string postalCode)
            {
                shippingAddressInput.PostalCode = postalCode;
                return this;
            }

            /// <summary>
            /// Sets the region/state.
            /// </summary>
            /// <param name="region">The region/state.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder Region(string region)
            {
                shippingAddressInput.Region = region;
                return this;
            }

            /// <summary>
            /// Sets the street address.
            /// </summary>
            /// <param name="streetAddress">The street address.</param>
            /// <returns>The builder instance.</returns>
            public ShippingAddressInputBuilder StreetAddress(string streetAddress)
            {
                shippingAddressInput.StreetAddress = streetAddress;
                return this;
            }

            public ShippingAddressInput Build()
            {
                return shippingAddressInput;
            }
        }
    }
}
