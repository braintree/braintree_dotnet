using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureLookupAdditionalInformation
    {
        public string AccountAgeIndicator { get; set; }
        public string AccountChangeDate { get; set; }
        public string AccountChangeIndicator { get; set; }
        public string AccountCreateDate { get; set; }
        public string AccountId { get; set; }
        public string AccountPurchases { get; set; }
        public string AccountPwdChangeDate { get; set; }
        public string AccountPwdChangeIndicator { get; set; }
        public string AddCardAttempts { get; set; }
        public string AddressMatch { get; set; }
        public string AuthenticationIndicator { get; set; }
        public string DeliveryEmail { get; set; }
        public string DeliveryTimeframe { get; set; }
        public string FraudActivity { get; set; }
        public string GiftCardAmount { get; set; }
        public string GiftCardCount { get; set; }
        public string GiftCardCurrencyCode { get; set; }
        public string Installment { get; set; }
        public string IpAddress { get; set; }
        public string OrderDescription { get; set; }
        public string PaymentAccountAge { get; set; }
        public string PaymentAccountIndicator { get; set; }
        public string PreorderDate { get; set; }
        public string PreorderIndicator { get; set; }
        public string ProductCode { get; set; }
        public string PurchaseDate { get; set; }
        public string RecurringEnd { get; set; }
        public string RecurringFrequency { get; set; }
        public string ReorderIndicator { get; set; }
        public ThreeDSecureLookupAddress ShippingAddress { get; set; }
        public string ShippingAddressUsageDate { get; set; }
        public string ShippingAddressUsageIndicator { get; set; }
        public string ShippingMethodIndicator { get; set; }
        public string ShippingNameIndicator { get; set; }
        public string TaxAmount { get; set; }
        public string TransactionCountDay { get; set; }
        public string TransactionCountYear { get; set; }
        public string UserAgent { get; set; }

        public Dictionary<string, object> ToDictionary() {
            var info = new Dictionary<string, object>();

            if (ShippingAddress != null) {
                info.Add("shipping_given_name", ShippingAddress.GivenName);
                info.Add("shipping_surname", ShippingAddress.Surname);
                info.Add("shipping_phone", ShippingAddress.PhoneNumber);
                info.Add("shipping_line1", ShippingAddress.StreetAddress);
                info.Add("shipping_line2", ShippingAddress.ExtendedAddress);
                info.Add("shipping_line3", ShippingAddress.Line3);
                info.Add("shipping_city", ShippingAddress.Locality);
                info.Add("shipping_state", ShippingAddress.Region);
                info.Add("shipping_postal_code", ShippingAddress.PostalCode);
                info.Add("shipping_country_code", ShippingAddress.CountryCodeAlpha2);
            }

            info.Add("shipping_method_indicator", ShippingMethodIndicator);
            info.Add("product_code", ProductCode);
            info.Add("delivery_timeframe", DeliveryTimeframe);
            info.Add("delivery_email", DeliveryEmail);
            info.Add("reorder_indicator", ReorderIndicator);
            info.Add("preorder_indicator", PreorderIndicator);
            info.Add("preorder_date", PreorderDate);
            info.Add("gift_card_amount", GiftCardAmount);
            info.Add("gift_card_currency_code", GiftCardCurrencyCode);
            info.Add("gift_card_count", GiftCardCount);
            info.Add("account_age_indicator", AccountAgeIndicator);
            info.Add("account_change_indicator", AccountChangeIndicator);
            info.Add("account_create_date", AccountCreateDate);
            info.Add("account_change_date", AccountChangeDate);
            info.Add("account_pwd_change_indicator", AccountPwdChangeIndicator);
            info.Add("account_pwd_change_date", AccountPwdChangeDate);
            info.Add("shipping_address_usage_indicator", ShippingAddressUsageIndicator);
            info.Add("shipping_address_usage_date", ShippingAddressUsageDate);
            info.Add("transaction_count_day", TransactionCountDay);
            info.Add("transaction_count_year", TransactionCountYear);
            info.Add("add_card_attempts", AddCardAttempts);
            info.Add("account_purchases", AccountPurchases);
            info.Add("fraud_activity", FraudActivity);
            info.Add("shipping_name_indicator", ShippingNameIndicator);
            info.Add("payment_account_indicator", PaymentAccountIndicator);
            info.Add("payment_account_age", PaymentAccountAge);
            info.Add("address_match", AddressMatch);
            info.Add("account_id", AccountId);
            info.Add("ip_address", IpAddress);
            info.Add("order_description", OrderDescription);
            info.Add("tax_amount", TaxAmount);
            info.Add("user_agent", UserAgent);
            info.Add("authentication_indicator", AuthenticationIndicator);
            info.Add("installment", Installment);
            info.Add("purchase_date", PurchaseDate);
            info.Add("recurring_end", RecurringEnd);
            info.Add("recurring_frequency", RecurringFrequency);

            return info;
        }

        protected internal ThreeDSecureLookupAdditionalInformation() {}
    }
}

