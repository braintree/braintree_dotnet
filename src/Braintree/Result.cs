#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public interface Result<out T>
    {
        CreditCardVerification CreditCardVerification { get; }
        Transaction Transaction { get; }
        Subscription Subscription { get; }
        ValidationErrors Errors { get; }
        Dictionary<string, string> Parameters { get; }
        string Message { get; }
        T Target { get; }
        bool IsSuccess();
    }

    public class ResultImpl<T> : Result<T> where T : class
    {
        public CreditCardVerification CreditCardVerification { get; protected set; }
        public Transaction Transaction { get; protected set; }
        public Subscription Subscription { get; protected set; }
        public ValidationErrors Errors { get; protected set; }
        public Dictionary<string, string> Parameters { get; protected set; }
        public string Message { get; protected set; }
        public T Target { get; protected set; }

        public ResultImpl(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node.IsSuccess())
            {
                Target = newInstanceFromResponse(node, gateway);
            }
            else
            {
                Errors = new ValidationErrors(node);
                NodeWrapper verificationNode = node.GetNode("verification");
                if (verificationNode != null) {
                    CreditCardVerification = new CreditCardVerification(verificationNode, gateway);
                }

                NodeWrapper transactionNode = node.GetNode("transaction");
                if (transactionNode != null)
                {
                    Transaction = new Transaction(transactionNode, gateway);
                }

                NodeWrapper subscriptionNode = node.GetNode("subscription");
                if (subscriptionNode != null)
                {
                    Subscription = new Subscription(subscriptionNode, gateway);
                }
                Parameters = node.GetNode("params").GetFormParameters();
                Message = node.GetString("message");
            }
        }

        public virtual bool IsSuccess()
        {
            return Errors == null;
        }

        private T newInstanceFromResponse(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (typeof(T) == typeof(Address))
            {
                return new Address(node) as T;
            }
            else if (typeof(T) == typeof(ApplePayCard))
            {
                return new ApplePayCard(node, gateway) as T;
            }
            else if (typeof(T) == typeof(AndroidPayCard))
            {
                return new AndroidPayCard(node, gateway) as T;
            }
            else if (typeof(T) == typeof(AmexExpressCheckoutCard))
            {
                return new AmexExpressCheckoutCard(node, gateway) as T;
            }
            else if (typeof(T) == typeof(CreditCard))
            {
                return new CreditCard(node, gateway) as T;
            }
            else if (typeof(T) == typeof(CreditCardVerification))
            {
                return new CreditCardVerification(node, gateway) as T;
            }
            else if (typeof(T) == typeof(CoinbaseAccount))
            {
                return new CoinbaseAccount(node, gateway) as T;
            }
            else if (typeof(T) == typeof(VenmoAccount))
            {
                return new VenmoAccount(node, gateway) as T;
            }
            else if (typeof(T) == typeof(Customer))
            {
                return new Customer(node, gateway) as T;
            }
            else if (typeof(T) == typeof(Transaction))
            {
                return new Transaction(node, gateway) as T;
            }
            else if (typeof(T) == typeof(Subscription))
            {
                return new Subscription(node, gateway) as T;
            }
            else if (typeof(T) == typeof(SettlementBatchSummary))
            {
                return new SettlementBatchSummary(node) as T;
            }
            else if (typeof(T) == typeof(MerchantAccount))
            {
                return new MerchantAccount(node) as T;
            }
            else if (typeof(T) == typeof(PayPalAccount))
            {
                return new PayPalAccount(node, gateway) as T;
            }
            else if (typeof(T) == typeof(UnknownPaymentMethod))
            {
                return new UnknownPaymentMethod(node) as T;
            }
            else if (typeof(T) == typeof(PaymentMethodNonce))
            {
                return new PaymentMethodNonce(node, gateway) as T;
            }
            else if (typeof(T) == typeof(OAuthCredentials))
            {
                return new OAuthCredentials(node) as T;
            }
            else if (typeof(T) == typeof(OAuthResult))
            {
                return new OAuthResult(node) as T;
            }
            else if (typeof(T) == typeof(Merchant))
            {
                return new Merchant(node) as T;
            }

            throw new Exception("Unknown T: " + typeof(T).ToString());
        }
    }
}
