#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// A class for building transaction requests with manual key entry.
    /// </summary>
    /// <example>
    /// A network tokenization attributes request can be constructed as follows:
    /// <code>
    /// NetworkTokenizationAttributesRequest createRequest = new NetworkTokenizationAttributesRequest
    /// {
    ///     Cryptogram = "validcryptogram",
    ///     EcommerceIndicator = "05",
    ///     TokenRequestorId = "123456"
    /// }
    /// </code>
    /// </example>
    public class NetworkTokenizationAttributesRequest : Request
    {
        public string Cryptogram { get; set; }
        public string EcommerceIndicator { get; set; }
        public string TokenRequestorId { get; set; }

        public override string ToXml()
        {
            return ToXml("network-tokenization-attributes");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("cryptogram", Cryptogram).
                AddElement("ecommerce-indicator", EcommerceIndicator).
                AddElement("token-requestor-id", TokenRequestorId);

        }
    }
}
