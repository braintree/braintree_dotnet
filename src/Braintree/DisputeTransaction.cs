using System;

namespace Braintree
{
    public class DisputeTransaction
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual int? InstallmentCount { get; protected set; }
        public virtual string OrderId { get; protected set; }
        public virtual string PaymentInstrumentSubtype { get; protected set; }
        public virtual string PurchaseOrderNumber { get; protected set; }

        protected internal DisputeTransaction(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            CreatedAt = node.GetDateTime("created-at");
            Id = node.GetString("id");
            InstallmentCount = node.GetInteger("installment-count");
            OrderId = node.GetString("order-id");
            PaymentInstrumentSubtype = node.GetString("payment-instrument-subtype");
            PurchaseOrderNumber = node.GetString("purchase-order-number");
        }

        [Obsolete("Mock Use Only")]
        protected internal DisputeTransaction() { }
    }
}

