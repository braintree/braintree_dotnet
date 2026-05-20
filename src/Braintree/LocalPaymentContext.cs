#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Braintree.GraphQL;

namespace Braintree
{
    /// <summary>
    /// Represents a Local Payment Context.
    /// </summary>
    public class LocalPaymentContext
    {
        public virtual MonetaryAmount Amount { get; protected set; }
        public virtual string ApprovedAt { get; protected set; }
        public virtual string ApprovalUrl { get; protected set; }
        public virtual string CreatedAt { get; protected set; }
        public virtual string ExpiredAt { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string LegacyId { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string OrderId { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string TransactedAt { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual string UpdatedAt { get; protected set; }

        protected internal LocalPaymentContext(Dictionary<string, object> response)
        {
            if (response.TryGetValue("id", out var id))
            {
                Id = id as string;
            }
            if (response.TryGetValue("legacyId", out var legacyId))
            {
                LegacyId = legacyId as string;
            }
            if (response.TryGetValue("type", out var type))
            {
                Type = type as string;
            }
            if (response.TryGetValue("paymentId", out var paymentId))
            {
                PaymentId = paymentId as string;
            }
            if (response.TryGetValue("orderId", out var orderId))
            {
                OrderId = orderId as string;
            }
            if (response.TryGetValue("approvalUrl", out var approvalUrl))
            {
                ApprovalUrl = approvalUrl as string;
            }
            if (response.TryGetValue("merchantAccountId", out var merchantAccountId))
            {
                MerchantAccountId = merchantAccountId as string;
            }
            if (response.TryGetValue("createdAt", out var createdAt))
            {
                CreatedAt = createdAt as string;
            }
            if (response.TryGetValue("updatedAt", out var updatedAt))
            {
                UpdatedAt = updatedAt as string;
            }
            if (response.TryGetValue("transactedAt", out var transactedAt))
            {
                TransactedAt = transactedAt as string;
            }
            if (response.TryGetValue("approvedAt", out var approvedAt))
            {
                ApprovedAt = approvedAt as string;
            }
            if (response.TryGetValue("expiredAt", out var expiredAt))
            {
                ExpiredAt = expiredAt as string;
            }
            if (response.TryGetValue("amount", out var amountObj))
            {
                Amount = ExtractAmount(amountObj as Dictionary<string, object>);
            }
        }

        private MonetaryAmount ExtractAmount(Dictionary<string, object> amountHash)
        {
            if (amountHash == null)
            {
                return null;
            }

            string currencyCode = null;
            if (amountHash.TryGetValue("currencyCode", out var cc))
            {
                currencyCode = cc as string;
            }
            else if (amountHash.TryGetValue("currencyIsoCode", out var cic))
            {
                currencyCode = cic as string;
            }

            decimal? value = null;
            if (amountHash.TryGetValue("value", out var valueObj))
            {
                if (valueObj is decimal decimalValue)
                {
                    value = decimalValue;
                }
                else if (valueObj is string stringValue)
                {
                    if (decimal.TryParse(stringValue, out var parsedValue))
                    {
                        value = parsedValue;
                    }
                }
            }

            if (value.HasValue)
            {
                return new MonetaryAmount(value.Value, currencyCode);
            }
            return null;
        }
    }
}
