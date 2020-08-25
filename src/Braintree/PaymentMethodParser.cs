using System;

namespace Braintree
{
    public class PaymentMethodParser
    {
        public static PaymentMethod ParsePaymentMethod(NodeWrapper response, IBraintreeGateway gateway)
        {
            if (response.GetName() == "paypal-account")
            {
                return new PayPalAccount(response, gateway);
            }
            else if (response.GetName() == "us-bank-account")
            {
                return new UsBankAccount(response);
            }
            else if (response.GetName() == "credit-card")
            {
                return new CreditCard(response, gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ApplePayCard(response, gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new AndroidPayCard(response, gateway);
            }
            else if (response.GetName() == "venmo-account")
            {
                return new VenmoAccount(response, gateway);
            }
            else if (response.GetName() == "visa-checkout-card")
            {
                return new VisaCheckoutCard(response, gateway);
            }
            else if (response.GetName() == "samsung-pay-card")
            {
                return new SamsungPayCard(response, gateway);
            }
            else
            {
                return new UnknownPaymentMethod(response);
            }
        }
    }
}
