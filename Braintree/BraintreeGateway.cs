#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// This is the primary interface to the Braintree Gateway.
    /// </summary>
    /// <remarks>
    /// This class interact with:
    /// <ul>
    /// <li><see cref="AddressGateway">Addresses</see></li>
    /// <li><see cref="CreditCardGateway">CreditCards</see></li>
    /// <li><see cref="CustomerGateway">Customers</see></li>
    /// <li><see cref="SubscriptionGateway">Subscriptions</see></li>
    /// <li><see cref="TransactionGateway">Transactions</see></li>
    /// </ul>
    /// </remarks>
    /// <example>
    /// Quick Start Example:
    /// <code>
    /// using System;
    /// using Braintree;
    ///
    /// namespace BraintreeExample
    /// {
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             var gateway = new BraintreeGateway
    ///             {
    ///                 Environment = Braintree.Environment.SANDBOX,
    ///                 MerchantId = "the_merchant_id",
    ///                 PublicKey = "a_public_key",
    ///                 PrivateKey = "a_private_key"
    ///             };
    ///
    ///             var request = new TransactionRequest
    ///             {
    ///                 Amount = 100.00M,
    ///                 CreditCard = new CreditCardRequest
    ///                 {
    ///                     Number = "5105105105105100",
    ///                     ExpirationDate = "05/12"
    ///                 }
    ///             };
    ///
    ///             Transaction transaction = gateway.Transaction.Sale(request).Target;
    ///
    ///             Console.WriteLine(String.Format("Transaction ID: {0}", transaction.Id));
    ///             Console.WriteLine(String.Format("Status: {0}", transaction.Status));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public class BraintreeGateway
    {
        public Environment Environment
        {
            get { return Configuration.Environment; }
            set { Configuration.Environment = value; }
        }

        public String MerchantId
        {
            get { return Configuration.MerchantId; }
            set { Configuration.MerchantId = value; }
        }

        public String PublicKey
        {
            get { return Configuration.PublicKey; }
            set { Configuration.PublicKey = value; }
        }

        public String PrivateKey
        {
            get { return Configuration.PrivateKey; }
            set { Configuration.PrivateKey = value; }
        }

        public Configuration Configuration { get; set; }

        public BraintreeGateway()
        {
            Configuration = new Configuration();
        }

        public BraintreeGateway(Environment environment, string merchantId, string publicKey, string privateKey)
        {
            Configuration = new Configuration(environment, merchantId, publicKey, privateKey);
        }

        public BraintreeGateway(Configuration configuration)
        {
            Configuration = configuration;
        }

        public virtual ClientTokenGateway ClientToken
        {
            get { return new ClientTokenGateway(new BraintreeService(Configuration)); }
        }

        public virtual CustomerGateway Customer
        {
            get { return new CustomerGateway(new BraintreeService(Configuration)); }
        }

        public virtual AddressGateway Address
        {
            get { return new AddressGateway(new BraintreeService(Configuration)); }
        }

        public virtual AddOnGateway AddOn
        {
            get { return new AddOnGateway(new BraintreeService(Configuration)); }
        }

        public virtual CreditCardGateway CreditCard
        {
            get { return new CreditCardGateway(new BraintreeService(Configuration)); }
        }

        public virtual CreditCardVerificationGateway CreditCardVerification
        {
            get { return new CreditCardVerificationGateway(new BraintreeService(Configuration)); }
        }

        public virtual DiscountGateway Discount
        {
            get { return new DiscountGateway(new BraintreeService(Configuration)); }
        }

        public virtual MerchantAccountGateway MerchantAccount
        {
            get { return new MerchantAccountGateway(new BraintreeService(Configuration)); }
        }

        public virtual PaymentMethodGateway PaymentMethod
        {
            get { return new PaymentMethodGateway(new BraintreeService(Configuration)); }
        }

        public virtual PayPalAccountGateway PayPalAccount
        {
            get { return new PayPalAccountGateway(new BraintreeService(Configuration)); }
        }

        public virtual PlanGateway Plan
        {
            get { return new PlanGateway(new BraintreeService(Configuration)); }
        }

        public virtual SettlementBatchSummaryGateway SettlementBatchSummary
        {
            get { return new SettlementBatchSummaryGateway(new BraintreeService(Configuration)); }
        }

        public virtual SubscriptionGateway Subscription
        {
            get { return new SubscriptionGateway(new BraintreeService(Configuration)); }
        }

        public virtual TransactionGateway Transaction
        {
            get { return new TransactionGateway(new BraintreeService(Configuration)); }
        }

        public virtual TransparentRedirectGateway TransparentRedirect
        {
            get { return new TransparentRedirectGateway(new BraintreeService(Configuration)); }
        }

        public virtual String TrData(Request trData, String redirectURL)
        {
            return TrUtil.BuildTrData(trData, redirectURL, new BraintreeService(Configuration));
        }

        public virtual WebhookNotificationGateway WebhookNotification
        {
            get { return new WebhookNotificationGateway(new BraintreeService(Configuration)); }
        }

        public virtual WebhookTestingGateway WebhookTesting
        {
            get { return new WebhookTestingGateway(new BraintreeService(Configuration)); }
        }

        public virtual PaymentMethodNonceGateway PaymentMethodNonce
        {
            get { return new PaymentMethodNonceGateway(new BraintreeService(Configuration)); }
        }
    }
}
