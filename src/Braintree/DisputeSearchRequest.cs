#pragma warning disable 1591

using System;

namespace Braintree
{
    public class DisputeSearchRequest : SearchRequest
    {
        public RangeNode<DisputeSearchRequest> AmountDisputed => new RangeNode<DisputeSearchRequest>("amount_disputed", this);

        public RangeNode<DisputeSearchRequest> AmountWon => new RangeNode<DisputeSearchRequest>("amount_won", this);

        public TextNode<DisputeSearchRequest> CaseNumber => new TextNode<DisputeSearchRequest>("case_number", this);

        public TextNode<DisputeSearchRequest> Id => new TextNode<DisputeSearchRequest>("id", this);

        public TextNode<DisputeSearchRequest> CustomerId => new TextNode<DisputeSearchRequest>("customer_id", this);

        public EnumMultipleValueNode<DisputeSearchRequest, DisputeKind> DisputeKind => new EnumMultipleValueNode<DisputeSearchRequest, DisputeKind>("kind", this);

        // NEXT_MAJOR_VERSION Remove this attribute
        [ObsoleteAttribute("use ProtectionLevel instead", false)]
        public EnumMultipleValueNode<DisputeSearchRequest, DisputeChargebackProtectionLevel> DisputeChargebackProtectionLevel => new EnumMultipleValueNode<DisputeSearchRequest, DisputeChargebackProtectionLevel>("chargeback_protection_level", this);

        public EnumMultipleValueNode<DisputeSearchRequest, DisputeProtectionLevel> DisputeProtectionLevel => new EnumMultipleValueNode<DisputeSearchRequest, DisputeProtectionLevel>("protection_level", this);

        public MultipleValueNode<DisputeSearchRequest, string> MerchantAccountId => new MultipleValueNode<DisputeSearchRequest, string>("merchant_account_id", this);

        public EnumMultipleValueNode<DisputeSearchRequest, DisputeReason> DisputeReason => new EnumMultipleValueNode<DisputeSearchRequest, DisputeReason>("reason", this);

        public MultipleValueNode<DisputeSearchRequest, string> ReasonCode => new MultipleValueNode<DisputeSearchRequest, string>("reason_code", this);

        public DateRangeNode<DisputeSearchRequest> ReceivedDate => new DateRangeNode<DisputeSearchRequest>("received_date", this);

        public DateRangeNode<DisputeSearchRequest> DisbursementDate => new DateRangeNode<DisputeSearchRequest>("disbursement_date", this);

        public DateRangeNode<DisputeSearchRequest> EffectiveDate => new DateRangeNode<DisputeSearchRequest>("effective_date", this);

        public TextNode<DisputeSearchRequest> ReferenceNumber => new TextNode<DisputeSearchRequest>("reference_number", this);

        public DateRangeNode<DisputeSearchRequest> ReplyByDate => new DateRangeNode<DisputeSearchRequest>("reply_by_date", this);

        public EnumMultipleValueNode<DisputeSearchRequest, DisputeStatus> DisputeStatus => new EnumMultipleValueNode<DisputeSearchRequest, DisputeStatus>("status", this);

        public TextNode<DisputeSearchRequest> TransactionId => new TextNode<DisputeSearchRequest>("transaction_id", this);

        public MultipleValueNode<DisputeSearchRequest, string> TransactionSource => new MultipleValueNode<DisputeSearchRequest, string>("transaction_source", this);
    }
}
