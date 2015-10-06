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
    ///             Console.WriteLine(string.Format("Transaction ID: {0}", transaction.Id));
    ///             Console.WriteLine(string.Format("Status: {0}", transaction.Status));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public class BraintreeGateway : IBraintreeGateway
    {
        public virtual Environment Environment
        {
            get { return Configuration.Environment; }
            set { Configuration.Environment = value; }
        }

        public virtual string MerchantId
        {
            get { return Configuration.MerchantId; }
            set { Configuration.MerchantId = value; }
        }

        public virtual string PublicKey
        {
            get { return Configuration.PublicKey; }
            set { Configuration.PublicKey = value; }
        }

        public virtual string PrivateKey
        {
            get { return Configuration.PrivateKey; }
            set { Configuration.PrivateKey = value; }
        }

        public virtual string AccessToken
        {
            get { return Configuration.AccessToken; }
            set { Configuration.AccessToken = value; }
        }

        public virtual string ClientId
        {
            get { return Configuration.ClientId; }
            set { Configuration.ClientId = value; }
        }

        public virtual string ClientSecret
        {
            get { return Configuration.ClientSecret; }
            set { Configuration.ClientSecret = value; }
        }

        private readonly Configuration configuration;
        public virtual Configuration Configuration { get { return configuration; } }

        public BraintreeGateway()
        {
            configuration = new Configuration();
        }

        public BraintreeGateway(Environment environment, string merchantId, string publicKey, string privateKey)
        {
            configuration = new Configuration(environment, merchantId, publicKey, privateKey);
        }

        public BraintreeGateway(string accessToken)
        {
            configuration = new Configuration(accessToken);
        }

        public BraintreeGateway(string clientId, string clientSecret)
        {
            configuration = new Configuration(clientId, clientSecret);
        }

        public BraintreeGateway(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public virtual IClientTokenGateway ClientToken
        {
            get { return new ClientTokenGateway(this); }
        }

        public virtual ICustomerGateway Customer
        {
            get { return new CustomerGateway(this); }
        }

        public virtual IAddressGateway Address
        {
            get { return new AddressGateway(this); }
        }

        public virtual IAddOnGateway AddOn
        {
            get { return new AddOnGateway(this); }
        }

        public virtual ICreditCardGateway CreditCard
        {
            get { return new CreditCardGateway(this); }
        }

        public virtual ICreditCardVerificationGateway CreditCardVerification
        {
            get { return new CreditCardVerificationGateway(this); }
        }

        public virtual IDiscountGateway Discount
        {
            get { return new DiscountGateway(this); }
        }

        public virtual IMerchantAccountGateway MerchantAccount
        {
            get { return new MerchantAccountGateway(this); }
        }

        public virtual IOAuthGateway OAuth
        {
            get { return new OAuthGateway(this); }
        }

        public virtual IMerchantGateway Merchant
        {
            get { return new MerchantGateway(this); }
        }

        public virtual IPaymentMethodGateway PaymentMethod
        {
            get { return new PaymentMethodGateway(this); }
        }

        public virtual IPayPalAccountGateway PayPalAccount
        {
            get { return new PayPalAccountGateway(this); }
        }

        public virtual IPlanGateway Plan
        {
            get { return new PlanGateway(this); }
        }

        public virtual ISettlementBatchSummaryGateway SettlementBatchSummary
        {
            get { return new SettlementBatchSummaryGateway(this); }
        }

        public virtual ISubscriptionGateway Subscription
        {
            get { return new SubscriptionGateway(this); }
        }

        public virtual ITestTransactionGateway TestTransaction
        {
            get { return new TestTransactionGateway(this); }
        }

        public virtual ITransactionGateway Transaction
        {
            get { return new TransactionGateway(this); }
        }

        public virtual ITransparentRedirectGateway TransparentRedirect
        {
            get { return new TransparentRedirectGateway(this); }
        }

        public virtual string TrData(Request trData, string redirectURL)
        {
            return TrUtil.BuildTrData(trData, redirectURL, new BraintreeService(Configuration));
        }

        public virtual IWebhookNotificationGateway WebhookNotification
        {
            get { return new WebhookNotificationGateway(this); }
        }

        public virtual IWebhookTestingGateway WebhookTesting
        {
            get { return new WebhookTestingGateway(this); }
        }

        public virtual IPaymentMethodNonceGateway PaymentMethodNonce
        {
            get { return new PaymentMethodNonceGateway(this); }
        }
    }
}
