#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides a fluent interface to build US bank account details for payment methods.
    /// </summary>
    public class PaymentMethodUsBankAccountRequest : Request
    {
        private string achMandateText;
        private DateTime? achMandateAcceptedAt;
        private PaymentMethodRequest parent;

        public PaymentMethodUsBankAccountRequest(PaymentMethodRequest parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Sets the ACH mandate text for Bank Account Instant Verification payment methods.
        /// </summary>
        /// <param name="achMandateText">the ACH mandate text</param>
        /// <returns>the PaymentMethodUsBankAccountRequest</returns>
        public PaymentMethodUsBankAccountRequest AchMandateText(string achMandateText)
        {
            this.achMandateText = achMandateText;
            return this;
        }

        /// <summary>
        /// Sets the timestamp when the ACH mandate was accepted.
        /// </summary>
        /// <param name="achMandateAcceptedAt">the timestamp when the mandate was accepted</param>
        /// <returns>the PaymentMethodUsBankAccountRequest</returns>
        public PaymentMethodUsBankAccountRequest AchMandateAcceptedAt(DateTime? achMandateAcceptedAt)
        {
            this.achMandateAcceptedAt = achMandateAcceptedAt;
            return this;
        }

        /// <summary>
        /// Returns control to the parent PaymentMethodRequest.
        /// </summary>
        /// <returns>the parent PaymentMethodRequest</returns>
        public PaymentMethodRequest Done()
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