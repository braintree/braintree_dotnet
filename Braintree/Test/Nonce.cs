using System;

namespace Braintree.Test
{
    public class Nonce
    {
        public const string Transactable = "fake-valid-nonce";
        public const string Consumed = "fake-consumed-nonce";
        public const string PayPalOneTimePayment = "fake-paypal-one-time-nonce";
        public const string PayPalFuturePayment = "fake-paypal-future-nonce";
        public const string ApplePayVisa = "fake-apple-pay-visa-nonce";
        public const string ApplePayMastercard = "fake-apple-pay-mastercard-nonce";
        public const string ApplePayAmex = "fake-apple-pay-amex-nonce";
        public const string AndroidPay = "fake-android-pay-nonce";
        public const string AbstractTransactable = "fake-abstract-transactable-nonce";
        public const string Europe = "fake-europe-bank-account-nonce";
        public const string Coinbase = "fake-coinbase-nonce";
    }
}
