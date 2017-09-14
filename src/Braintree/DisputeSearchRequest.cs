#pragma warning disable 1591

namespace Braintree
{
    public class DisputeSearchRequest : SearchRequest
    {
        public RangeNode<DisputeSearchRequest> AmountDisputed
        {
            get
            {
                return new RangeNode<DisputeSearchRequest>("amount_disputed", this);
            }
        }

        public RangeNode<DisputeSearchRequest> AmountWon
        {
            get
            {
                return new RangeNode<DisputeSearchRequest>("amount_won", this);
            }
        }

        public TextNode<DisputeSearchRequest> CaseNumber
        {
            get
            {
                return new TextNode<DisputeSearchRequest>("case_number", this);
            }
        }

        public TextNode<DisputeSearchRequest> Id
        {
            get
            {
                return new TextNode<DisputeSearchRequest>("id", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, DisputeKind> DisputeKind
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, DisputeKind>("kind", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, string> MerchantAccountId
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, string>("merchant_account_id", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, DisputeReason> DisputeReason
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, DisputeReason>("reason", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, string> ReasonCode
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, string>("reason_code", this);
            }
        }

        public DateRangeNode<DisputeSearchRequest> ReceivedDate
        {
            get
            {
                return new DateRangeNode<DisputeSearchRequest>("received_date", this);
            }
        }

        public TextNode<DisputeSearchRequest> ReferenceNumber
        {
            get
            {
                return new TextNode<DisputeSearchRequest>("reference_number", this);
            }
        }

        public DateRangeNode<DisputeSearchRequest> ReplyByDate
        {
            get
            {
                return new DateRangeNode<DisputeSearchRequest>("reply_by_date", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, DisputeStatus> DisputeStatus
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, DisputeStatus>("status", this);
            }
        }

        public TextNode<DisputeSearchRequest> TransactionId
        {
            get
            {
                return new TextNode<DisputeSearchRequest>("transaction_id", this);
            }
        }

        public MultipleValueNode<DisputeSearchRequest, string> TransactionSource
        {
            get
            {
                return new MultipleValueNode<DisputeSearchRequest, string>("transaction_source", this);
            }
        }
    }
}
