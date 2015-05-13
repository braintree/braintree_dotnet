using System;

namespace Braintree.Test
{
    public class Nonce
    {
        public const String Transactable = "fake-valid-nonce";
        public const String Consumed = "fake-consumed-nonce";
        public const String PayPalOneTimePayment = "fake-paypal-one-time-nonce";
        public const String PayPalFuturePayment = "fake-paypal-future-nonce";
        public const String ApplePayVisa = "fake-apple-pay-visa-nonce";
        public const String ApplePayMastercard = "fake-apple-pay-mastercard-nonce";
        public const String ApplePayAmex = "fake-apple-pay-amex-nonce";
        public const String AndroidPay = "fake-android-pay-nonce";
        public const String AbstractTransactable = "fake-abstract-transactable-nonce";
        public const String Europe = "fake-europe-bank-account-nonce";
        public const String Coinbase = "fake-coinbase-nonce";
    }
}
