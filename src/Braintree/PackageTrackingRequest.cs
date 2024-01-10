#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree
{
    public class PackageTrackingRequest : Request
    {
        public string Carrier { get; set; }
        public TransactionLineItemRequest[] LineItems { get; set; }
        public bool NotifyPayer { get; set; }
        public string TrackingNumber { get; set; }

        /// <summary>
        /// A class for building requests to manipulate <see cref="PackageTrackingRequest"/> records in the vault.
        /// </summary>
        /// <example>
        /// A transaction request can be constructed as follows:
        /// <code>
        /// var request = new PackageTrackingRequest
        /// {
        ///     Carrier = FEDEX,
        ///     LineItems = new TransactionLineItemRequest[],
        ///     NotifyPayer = false,
        ///     Tracking Carrier Enum - https://developer.paypal.com/docs/tracking/reference/carriers/
        ///     TrackingNumber = "123123ZLKJSAD321",
        ///    {
        ///        new TransactionLineItemRequest
        ///        {
        ///            Quantity = quantities[i],
        ///            Name = "Name #1",
        ///            LineItemKind = TransactionLineItemKind.DEBIT,
        ///            UnitAmount = 45.1232M,
        ///            TotalAmount = 45.15M,
        ///        }
        ///    }
        /// };
        /// </code>
        /// </example>
        public PackageTrackingRequest() { }

        public override string ToXml()
        {
            return ToXml("shipment");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("shipment");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("carrier", Carrier);
            if (LineItems != null)
            {
                builder.AddElement("line-items", LineItems);
            }
            builder.AddElement("notify-payer", NotifyPayer);
            builder.AddElement("tracking-number", TrackingNumber);
            return builder;
        }
    }
}