#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides a fluent interface to build US bank account details for transactions.
    /// </summary>
    public class TransactionUsBankAccountRequest : Request
    {
        private string achMandateText;
        private DateTime? achMandateAcceptedAt;
        private TransactionRequest parent;

        public TransactionUsBankAccountRequest(TransactionRequest parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Sets the ACH mandate text for Bank Account Instant Verification transactions.
        /// </summary>
        /// <param name="achMandateText">the ACH mandate text</param>
        /// <returns>the TransactionUsBankAccountRequest</returns>
        public TransactionUsBankAccountRequest AchMandateText(string achMandateText)
        {
            this.achMandateText = achMandateText;
            return this;
        }

        /// <summary>
        /// Sets the timestamp when the ACH mandate was accepted.
        /// </summary>
        /// <param name="achMandateAcceptedAt">the timestamp when the mandate was accepted</param>
        /// <returns>the TransactionUsBankAccountRequest</returns>
        public TransactionUsBankAccountRequest AchMandateAcceptedAt(DateTime? achMandateAcceptedAt)
        {
            this.achMandateAcceptedAt = achMandateAcceptedAt;
            return this;
        }

        /// <summary>
        /// Returns control to the parent TransactionRequest.
        /// </summary>
        /// <returns>the parent TransactionRequest</returns>
        public TransactionRequest Done()
        {
            return parent;
        }

        public string GetAchMandateText()
        {
            return achMandateText;
        }

        public DateTime? GetAchMandateAcceptedAt()
        {
            return achMandateAcceptedAt;
        }

        public override string ToXml()
        {
            return BuildRequest("usBankAccount").ToXml();
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("usBankAccount");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root)
                .AddElement("achMandateText", achMandateText)
                .AddElement("achMandateAcceptedAt", achMandateAcceptedAt);
        }
    }
}