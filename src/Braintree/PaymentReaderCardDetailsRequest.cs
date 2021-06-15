#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// A class for building transaction requests with manual key entry.
    /// </summary>
    /// <example>
    /// A payment reader card details request can be constructed as follows:
    /// <code>
    /// PaymentReaderCardDetailsRequest createRequest = new PaymentReaderCardDetailsRequest
    /// {
    ///     EncryptedCardData = "8F34DFB312DC79C24FD5320622F3E11682D79E6B0C0FD881",
    ///     KeySerialNumber = "FFFFFF02000572A00005",
    /// }
    /// </code>
    /// </example>
    public class PaymentReaderCardDetailsRequest : Request
    {
        public string EncryptedCardData { get; set; }
        public string KeySerialNumber { get; set; }

        public override string ToXml()
        {
            return ToXml("payment-reader-card-details");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("encrypted-card-data", EncryptedCardData).
                AddElement("key-serial-number", KeySerialNumber);
        }
    }
}
