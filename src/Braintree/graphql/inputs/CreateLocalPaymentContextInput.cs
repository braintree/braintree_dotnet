using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Input fields for creating a local payment context.
    /// </summary>
    public class CreateLocalPaymentContextInput
    {
        public virtual MonetaryAmountInput Amount { get; protected set; }
        public virtual string CancelUrl { get; protected set; }
        public virtual string CountryCode { get; protected set; }
        public virtual string ExpiryDate { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string OrderId { get; protected set; }
        public virtual PayerInfoInput PayerInfo { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string ReturnUrl { get; protected set; }
        public virtual string Type { get; protected set; }

        /// <returns>
        /// A dictionary representing the input object wrapped in a paymentContext key, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var paymentContext = new Dictionary<string, object>();

            if (Amount != null)
            {
                paymentContext["amount"] = Amount.ToGraphQLVariables();
            }
            if (Type != null)
            {
                paymentContext["type"] = Type;
            }
            if (PayerInfo != null)
            {
                paymentContext["payerInfo"] = PayerInfo.ToGraphQLVariables();
            }
            if (ReturnUrl != null)
            {
                paymentContext["returnUrl"] = ReturnUrl;
            }
            if (CancelUrl != null)
            {
                paymentContext["cancelUrl"] = CancelUrl;
            }
            if (MerchantAccountId != null)
            {
                paymentContext["merchantAccountId"] = MerchantAccountId;
            }
            if (OrderId != null)
            {
                paymentContext["orderId"] = OrderId;
            }
            if (CountryCode != null)
            {
                paymentContext["countryCode"] = CountryCode;
            }
            if (ExpiryDate != null)
            {
                paymentContext["expiryDate"] = ExpiryDate;
            }
            if (PaymentId != null)
            {
                paymentContext["paymentId"] = PaymentId;
            }

            var variables = new Dictionary<string, object>();
            variables["paymentContext"] = paymentContext;
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="CreateLocalPaymentContextInput"/>.
        /// </summary>
        /// <returns>A <see cref="CreateLocalPaymentContextInputBuilder"/> instance.</returns>
        public static CreateLocalPaymentContextInputBuilder Builder()
        {
            return new CreateLocalPaymentContextInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CreateLocalPaymentContextInput"/>.
        /// </summary>
        public class CreateLocalPaymentContextInputBuilder
        {
            private CreateLocalPaymentContextInput createLocalPaymentContextInput = new CreateLocalPaymentContextInput();

            /// <summary>
            /// Sets the amount.
            /// </summary>
            /// <param name="amount">The amount.</param>
            /// <param name="currencyIsoCode">The currency ISO code.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder Amount(decimal amount, string currencyIsoCode = null)
            {
                createLocalPaymentContextInput.Amount = new MonetaryAmountInput(amount, currencyIsoCode);
                return this;
            }

            /// <summary>
            /// Sets the amount input.
            /// </summary>
            /// <param name="amount">The amount input.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder AmountInput(MonetaryAmountInput amount)
            {
                createLocalPaymentContextInput.Amount = amount;
                return this;
            }

            /// <summary>
            /// Sets the cancel URL.
            /// </summary>
            /// <param name="cancelUrl">The cancel URL.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder CancelUrl(string cancelUrl)
            {
                createLocalPaymentContextInput.CancelUrl = cancelUrl;
                return this;
            }

            /// <summary>
            /// Sets the country code.
            /// </summary>
            /// <param name="countryCode">The country code.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder CountryCode(string countryCode)
            {
                createLocalPaymentContextInput.CountryCode = countryCode;
                return this;
            }

            /// <summary>
            /// Sets the expiry date.
            /// </summary>
            /// <param name="expiryDate">The expiry date.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder ExpiryDate(string expiryDate)
            {
                createLocalPaymentContextInput.ExpiryDate = expiryDate;
                return this;
            }

            /// <summary>
            /// Sets the merchant account ID.
            /// </summary>
            /// <param name="merchantAccountId">The merchant account ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder MerchantAccountId(string merchantAccountId)
            {
                createLocalPaymentContextInput.MerchantAccountId = merchantAccountId;
                return this;
            }

            /// <summary>
            /// Sets the order ID.
            /// </summary>
            /// <param name="orderId">The order ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder OrderId(string orderId)
            {
                createLocalPaymentContextInput.OrderId = orderId;
                return this;
            }

            /// <summary>
            /// Sets the payer info.
            /// </summary>
            /// <param name="payerInfo">The payer info.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder PayerInfo(PayerInfoInput payerInfo)
            {
                createLocalPaymentContextInput.PayerInfo = payerInfo;
                return this;
            }

            /// <summary>
            /// Sets the payment ID.
            /// </summary>
            /// <param name="paymentId">The payment ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder PaymentId(string paymentId)
            {
                createLocalPaymentContextInput.PaymentId = paymentId;
                return this;
            }

            /// <summary>
            /// Sets the return URL.
            /// </summary>
            /// <param name="returnUrl">The return URL.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder ReturnUrl(string returnUrl)
            {
                createLocalPaymentContextInput.ReturnUrl = returnUrl;
                return this;
            }

            /// <summary>
            /// Sets the type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>The builder instance.</returns>
            public CreateLocalPaymentContextInputBuilder Type(string type)
            {
                createLocalPaymentContextInput.Type = type;
                return this;
            }

            public CreateLocalPaymentContextInput Build()
            {
                return createLocalPaymentContextInput;
            }
        }
    }
}
