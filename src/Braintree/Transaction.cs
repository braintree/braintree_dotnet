#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class TransactionGatewayRejectionReason : Enumeration
    {
        public static readonly TransactionGatewayRejectionReason APPLICATION_INCOMPLETE = new TransactionGatewayRejectionReason("application_incomplete");
        public static readonly TransactionGatewayRejectionReason AVS = new TransactionGatewayRejectionReason("avs");
        public static readonly TransactionGatewayRejectionReason AVS_AND_CVV = new TransactionGatewayRejectionReason("avs_and_cvv");
        public static readonly TransactionGatewayRejectionReason CVV = new TransactionGatewayRejectionReason("cvv");
        public static readonly TransactionGatewayRejectionReason DUPLICATE = new TransactionGatewayRejectionReason("duplicate");
        public static readonly TransactionGatewayRejectionReason FRAUD = new TransactionGatewayRejectionReason("fraud");
        public static readonly TransactionGatewayRejectionReason THREE_D_SECURE = new TransactionGatewayRejectionReason("three_d_secure");
        public static readonly TransactionGatewayRejectionReason TOKEN_ISSUANCE = new TransactionGatewayRejectionReason("token_issuance");
        public static readonly TransactionGatewayRejectionReason UNRECOGNIZED = new TransactionGatewayRejectionReason("unrecognized");

        public static readonly TransactionGatewayRejectionReason[] ALL = {
            APPLICATION_INCOMPLETE, AVS, AVS_AND_CVV, CVV, DUPLICATE, FRAUD, THREE_D_SECURE, TOKEN_ISSUANCE, UNRECOGNIZED
        };

        protected TransactionGatewayRejectionReason(string name) : base(name) {}
    }

    public class TransactionEscrowStatus : Enumeration
    {
        public static readonly TransactionEscrowStatus HOLD_PENDING = new TransactionEscrowStatus("hold_pending");
        public static readonly TransactionEscrowStatus HELD = new TransactionEscrowStatus("held");
        public static readonly TransactionEscrowStatus RELEASE_PENDING = new TransactionEscrowStatus("release_pending");
        public static readonly TransactionEscrowStatus RELEASED = new TransactionEscrowStatus("released");
        public static readonly TransactionEscrowStatus REFUNDED = new TransactionEscrowStatus("refunded");
        public static readonly TransactionEscrowStatus UNRECOGNIZED = new TransactionEscrowStatus("unrecognized");

        public static readonly TransactionEscrowStatus[] ALL = {
            HELD, HOLD_PENDING, RELEASE_PENDING, RELEASED, REFUNDED, UNRECOGNIZED
        };

        protected TransactionEscrowStatus(string name) : base(name) {}
    }

    public class TransactionStatus : Enumeration
    {
        public static readonly TransactionStatus AUTHORIZATION_EXPIRED = new TransactionStatus("authorization_expired");
        public static readonly TransactionStatus AUTHORIZED = new TransactionStatus("authorized");
        public static readonly TransactionStatus AUTHORIZING = new TransactionStatus("authorizing");
        public static readonly TransactionStatus FAILED = new TransactionStatus("failed");
        public static readonly TransactionStatus GATEWAY_REJECTED = new TransactionStatus("gateway_rejected");
        public static readonly TransactionStatus PROCESSOR_DECLINED = new TransactionStatus("processor_declined");
        public static readonly TransactionStatus SETTLED = new TransactionStatus("settled");
        public static readonly TransactionStatus SETTLING = new TransactionStatus("settling");
        public static readonly TransactionStatus SUBMITTED_FOR_SETTLEMENT = new TransactionStatus("submitted_for_settlement");
        public static readonly TransactionStatus VOIDED = new TransactionStatus("voided");
        public static readonly TransactionStatus UNRECOGNIZED = new TransactionStatus("unrecognized");
        public static readonly TransactionStatus SETTLEMENT_CONFIRMED = new TransactionStatus("settlement_confirmed");
        public static readonly TransactionStatus SETTLEMENT_DECLINED = new TransactionStatus("settlement_declined");
        public static readonly TransactionStatus SETTLEMENT_PENDING = new TransactionStatus("settlement_pending");

        public static readonly TransactionStatus[] ALL = {
            AUTHORIZATION_EXPIRED, AUTHORIZED, AUTHORIZING, FAILED, GATEWAY_REJECTED, PROCESSOR_DECLINED,
            SETTLED, SETTLEMENT_CONFIRMED, SETTLEMENT_DECLINED, SETTLEMENT_PENDING, SETTLING, SUBMITTED_FOR_SETTLEMENT, VOIDED, UNRECOGNIZED
        };

        protected TransactionStatus(string name) : base(name) {}
    }

    public class TransactionIndustryType : Enumeration
    {
        public static readonly TransactionIndustryType LODGING = new TransactionIndustryType("lodging");
        public static readonly TransactionIndustryType TRAVEL_AND_CRUISE = new TransactionIndustryType("travel_cruise");
        public static readonly TransactionIndustryType TRAVEL_AND_FLIGHT = new TransactionIndustryType("travel_flight");

        protected TransactionIndustryType(string name) : base(name) {}
    }

    public class TransactionSource : Enumeration
    {
        public static readonly TransactionSource API = new TransactionSource("api");
        public static readonly TransactionSource CONTROL_PANEL = new TransactionSource("control_panel");
        public static readonly TransactionSource RECURRING = new TransactionSource("recurring");
        public static readonly TransactionSource UNRECOGNIZED = new TransactionSource("unrecognized");

        public static readonly TransactionSource[] ALL = { API, CONTROL_PANEL, RECURRING, UNRECOGNIZED };

        protected TransactionSource(string name) : base(name) {}
    }

    public class TransactionType : Enumeration
    {
        public static readonly TransactionType CREDIT = new TransactionType("credit");
        public static readonly TransactionType SALE = new TransactionType("sale");
        public static readonly TransactionType UNRECOGNIZED = new TransactionType("unrecognized");

        public static readonly TransactionType[] ALL = { CREDIT, SALE, UNRECOGNIZED };

        protected TransactionType(string name) : base(name) {}
    }

    public class TransactionCreatedUsing : Enumeration
    {
        public static readonly TransactionCreatedUsing FULL_INFORMATION = new TransactionCreatedUsing("full_information");
        public static readonly TransactionCreatedUsing TOKEN = new TransactionCreatedUsing("token");
        public static readonly TransactionCreatedUsing UNRECOGNIZED = new TransactionCreatedUsing("unrecognized");

        public static readonly TransactionCreatedUsing[] ALL = { FULL_INFORMATION, TOKEN, UNRECOGNIZED };

        protected TransactionCreatedUsing(string name) : base(name) {}
    }


    public class PaymentInstrumentType : Enumeration
    {
        public static readonly PaymentInstrumentType PAYPAL_ACCOUNT = new PaymentInstrumentType("paypal_account");
        public static readonly PaymentInstrumentType PAYPAL_HERE = new PaymentInstrumentType("paypal_here");
        public static readonly PaymentInstrumentType EUROPE_BANK_ACCOUNT= new PaymentInstrumentType("europe_bank_account");
        public static readonly PaymentInstrumentType CREDIT_CARD = new PaymentInstrumentType("credit_card");
        public static readonly PaymentInstrumentType COINBASE_ACCOUNT= new PaymentInstrumentType("coinbase_account");
        public static readonly PaymentInstrumentType APPLE_PAY_CARD = new PaymentInstrumentType("apple_pay_card");
        public static readonly PaymentInstrumentType ANDROID_PAY_CARD = new PaymentInstrumentType("android_pay_card");
        public static readonly PaymentInstrumentType AMEX_EXPRESS_CHECKOUT_CARD = new PaymentInstrumentType("amex_express_checkout_card");
        public static readonly PaymentInstrumentType VENMO_ACCOUNT = new PaymentInstrumentType("venmo_account");
        public static readonly PaymentInstrumentType US_BANK_ACCOUNT = new PaymentInstrumentType("us_bank_account");
        public static readonly PaymentInstrumentType VISA_CHECKOUT_CARD = new PaymentInstrumentType("visa_checkout_card");
        public static readonly PaymentInstrumentType MASTERPASS_CARD = new PaymentInstrumentType("masterpass_card");
        public static readonly PaymentInstrumentType SAMSUNG_PAY_CARD = new PaymentInstrumentType("samsung_pay_card");
        public static readonly PaymentInstrumentType LOCAL_PAYMENT = new PaymentInstrumentType("local_payment");
        // NEXT_MAJOR_VERSION Remove this class as legacy Ideal has been removed/disabled in the Braintree Gateway
        // DEPRECATED If you're looking to accept iDEAL as a payment method contact accounts@braintreepayments.com for a solution.
        public static readonly PaymentInstrumentType IDEAL_PAYMENT = new PaymentInstrumentType("ideal_payment");
        public static readonly PaymentInstrumentType ANY = new PaymentInstrumentType("any");
        public static readonly PaymentInstrumentType UNKNOWN = new PaymentInstrumentType("unknown");

        public static readonly PaymentInstrumentType[] ALL = { PAYPAL_ACCOUNT, PAYPAL_HERE, EUROPE_BANK_ACCOUNT, CREDIT_CARD, COINBASE_ACCOUNT, ANDROID_PAY_CARD, APPLE_PAY_CARD, AMEX_EXPRESS_CHECKOUT_CARD, VENMO_ACCOUNT, US_BANK_ACCOUNT, VISA_CHECKOUT_CARD, MASTERPASS_CARD, IDEAL_PAYMENT, LOCAL_PAYMENT, ANY, UNKNOWN };

        protected PaymentInstrumentType(string name) : base(name) {}
    }

    /// <summary>
    /// A transaction returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Transactions can be retrieved via the gateway using the associated transaction id:
    /// <code>
    ///     Transaction transaction = gateway.Transaction.Find("transactionId");
    /// </code>
    /// For more information about Transactions, see <a href="https://developers.braintreepayments.com/reference/response/transaction/dotnet" target="_blank">https://developers.braintreepayments.com/reference/response/transaction/dotnet</a>
    /// </example>
    public class Transaction
    {
        public virtual string Id { get; protected set; }
        public virtual List<AddOn> AddOns { get; protected set; }
        public virtual decimal? Amount { get; protected set; }
        public virtual string AvsErrorResponseCode { get; protected set; }
        public virtual string AvsPostalCodeResponseCode { get; protected set; }
        public virtual string AvsStreetAddressResponseCode { get; protected set; }
        public virtual Address BillingAddress { get; protected set; }
        public virtual string Channel { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual CreditCard CreditCard { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual CustomerDetails CustomerDetails { get; protected set; }
        public virtual string CvvResponseCode { get; protected set; }
        public virtual Descriptor Descriptor { get; protected set; }
        public virtual List<Discount> Discounts { get; protected set; }
        public virtual List<Dispute> Disputes { get; protected set; }
        public virtual TransactionGatewayRejectionReason GatewayRejectionReason { get; protected set; }
        public virtual string GraphQLId { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string OrderId { get; protected set; }
        public virtual string PlanId { get; protected set; }
        public virtual string ProcessorAuthorizationCode { get; protected set; }
        public virtual ProcessorResponseType ProcessorResponseType { get; protected set; }
        public virtual string ProcessorResponseCode { get; protected set; }
        public virtual string ProcessorResponseText { get; protected set; }
        public virtual string ProcessorSettlementResponseCode { get; protected set; }
        public virtual string ProcessorSettlementResponseText { get; protected set; }
        public virtual string AdditionalProcessorResponse { get; protected set; }
        public virtual string NetworkResponseCode { get; protected set; }
        public virtual string NetworkResponseText { get; protected set; }
        public virtual string VoiceReferralNumber { get; protected set; }
        public virtual string PurchaseOrderNumber { get; protected set; }
        public virtual bool? Recurring { get; protected set; }
        public virtual string RefundedTransactionId { get; protected set; }
        public virtual List<string> RefundIds { get; protected set; }
        public virtual List<string> PartialSettlementTransactionIds { get; protected set; }
        public virtual string AuthorizedTransactionId { get; protected set; }
        public virtual string SettlementBatchId { get; protected set; }
        public virtual Address ShippingAddress { get; protected set; }
        public virtual TransactionEscrowStatus EscrowStatus { get; protected set; }
        public virtual TransactionStatus Status { get; protected set; }
        public virtual StatusEvent[] StatusHistory { get; protected set; }
        public virtual List<AuthorizationAdjustment> AuthorizationAdjustments { get; protected set; }
        public virtual string SubscriptionId { get; protected set; }
        public virtual SubscriptionDetails SubscriptionDetails { get; protected set; }
        public virtual decimal? TaxAmount { get; protected set; }
        public virtual bool? TaxExempt { get; protected set; }
        public virtual TransactionType Type { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Dictionary<string, string> CustomFields { get; protected set; }
        public virtual decimal? ServiceFeeAmount { get; protected set; }
        public virtual DisbursementDetails DisbursementDetails { get; protected set; }
        public virtual ApplePayDetails ApplePayDetails { get; protected set; }
        public virtual AndroidPayDetails AndroidPayDetails { get; protected set; }
        public virtual AmexExpressCheckoutDetails AmexExpressCheckoutDetails { get; protected set; }
        public virtual PayPalDetails PayPalDetails { get; protected set; }
        public virtual PayPalHereDetails PayPalHereDetails { get; protected set; }
        public virtual LocalPaymentDetails LocalPaymentDetails { get; protected set; }
        public virtual CoinbaseDetails CoinbaseDetails { get; protected set; }
        public virtual VenmoAccountDetails VenmoAccountDetails { get; protected set; }
        public virtual UsBankAccountDetails UsBankAccountDetails { get; protected set; }
        // NEXT_MAJOR_VERSION Remove this class as legacy Ideal has been removed/disabled in the Braintree Gateway
        // DEPRECATED If you're looking to accept iDEAL as a payment method contact accounts@braintreepayments.com for a solution.
        public virtual IdealPaymentDetails IdealPaymentDetails { get; protected set; }
        public virtual VisaCheckoutCardDetails VisaCheckoutCardDetails { get; protected set; }
        public virtual MasterpassCardDetails MasterpassCardDetails { get; protected set; }
        public virtual SamsungPayCardDetails SamsungPayCardDetails { get; protected set; }
        public virtual PaymentInstrumentType PaymentInstrumentType { get; protected set; }
        public virtual RiskData RiskData { get; protected set; }
        public virtual ThreeDSecureInfo ThreeDSecureInfo { get; protected set; }
        public virtual FacilitatedDetails FacilitatedDetails { get; protected set; }
        public virtual FacilitatorDetails FacilitatorDetails { get; protected set; }
        public virtual decimal? DiscountAmount { get; protected set; }
        public virtual decimal? ShippingAmount { get; protected set; }
        public virtual string ShipsFromPostalCode { get; protected set; }
        public virtual string NetworkTransactionId { get; protected set; }
        public virtual DateTime? AuthorizationExpiresAt { get; protected set; }
        public virtual string RetrievalReferenceNumber { get; protected set; }

        private IBraintreeGateway Gateway;

        [Obsolete("Mock Use Only")]
        protected Transaction() { }

        protected internal Transaction(NodeWrapper node, IBraintreeGateway gateway)
        {
            Gateway = gateway;

            if (node == null)
                return;

            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            AvsErrorResponseCode = node.GetString("avs-error-response-code");
            AvsPostalCodeResponseCode = node.GetString("avs-postal-code-response-code");
            AvsStreetAddressResponseCode = node.GetString("avs-street-address-response-code");
            GatewayRejectionReason = (TransactionGatewayRejectionReason)CollectionUtil.Find(
                TransactionGatewayRejectionReason.ALL,
                node.GetString("gateway-rejection-reason"),
                TransactionGatewayRejectionReason.UNRECOGNIZED
            );
            PaymentInstrumentType = (PaymentInstrumentType)CollectionUtil.Find(
                PaymentInstrumentType.ALL,
                node.GetString("payment-instrument-type"),
                PaymentInstrumentType.UNKNOWN
            );
            Channel = node.GetString("channel");
            GraphQLId = node.GetString("global-id");
            OrderId = node.GetString("order-id");
            Status = (TransactionStatus)CollectionUtil.Find(TransactionStatus.ALL, node.GetString("status"), TransactionStatus.UNRECOGNIZED);
            EscrowStatus = (TransactionEscrowStatus)CollectionUtil.Find(
                    TransactionEscrowStatus.ALL,
                    node.GetString("escrow-status"),
                    TransactionEscrowStatus.UNRECOGNIZED
            );

            List<NodeWrapper> statusNodes = node.GetList("status-history/status-event");
            StatusHistory = new StatusEvent[statusNodes.Count];
            for (int i = 0; i < statusNodes.Count; i++)
            {
                StatusHistory[i] = new StatusEvent(statusNodes[i]);
            }

            Type = (TransactionType)CollectionUtil.Find(TransactionType.ALL, node.GetString("type"), TransactionType.UNRECOGNIZED);
            MerchantAccountId = node.GetString("merchant-account-id");
            ProcessorAuthorizationCode = node.GetString("processor-authorization-code");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            ProcessorResponseType = (ProcessorResponseType)CollectionUtil.Find(ProcessorResponseType.ALL, node.GetString("processor-response-type"), ProcessorResponseType.UNRECOGNIZED);
            ProcessorSettlementResponseCode = node.GetString("processor-settlement-response-code");
            ProcessorSettlementResponseText = node.GetString("processor-settlement-response-text");
            NetworkResponseCode = node.GetString("network-response-code");
            NetworkResponseText = node.GetString("network-response-text");
            AdditionalProcessorResponse = node.GetString("additional-processor-response");
            VoiceReferralNumber = node.GetString("voice-referral-number");
            PurchaseOrderNumber = node.GetString("purchase-order-number");
            Recurring = node.GetBoolean("recurring");
            RefundedTransactionId = node.GetString("refunded-transaction-id");

            RefundIds = node.GetStrings("refund-ids/*");
            PartialSettlementTransactionIds = node.GetStrings("partial-settlement-transaction-ids/*");
            AuthorizedTransactionId = node.GetString("authorized-transaction-id");
            SettlementBatchId = node.GetString("settlement-batch-id");
            PlanId = node.GetString("plan-id");
            SubscriptionId = node.GetString("subscription-id");
            TaxAmount = node.GetDecimal("tax-amount");
            TaxExempt = node.GetBoolean("tax-exempt");
            CustomFields = node.GetDictionary("custom-fields");
            var creditCardNode = node.GetNode("credit-card");
            if (creditCardNode != null)
            {
                CreditCard = new CreditCard(creditCardNode, gateway);
            }
            var subscriptionNode = node.GetNode("subscription");
            if (subscriptionNode != null)
            {
                SubscriptionDetails = new SubscriptionDetails(subscriptionNode);
            }
            var customerNode = node.GetNode("customer");
            if (customerNode != null)
            {
                CustomerDetails = new CustomerDetails(customerNode, gateway);
            }
            CurrencyIsoCode = node.GetString("currency-iso-code");
            CvvResponseCode = node.GetString("cvv-response-code");
            var descriptorNode = node.GetNode("descriptor");
            if (descriptorNode != null)
            {
                Descriptor = new Descriptor(descriptorNode);
            }
            ServiceFeeAmount = node.GetDecimal("service-fee-amount");
            var disbursementDetailsNode = node.GetNode("disbursement-details");
            if (disbursementDetailsNode != null)
            {
                DisbursementDetails = new DisbursementDetails(disbursementDetailsNode);
            }
            var paypalNode = node.GetNode("paypal");
            if (paypalNode != null)
            {
                PayPalDetails = new PayPalDetails(paypalNode);
            }
            var paypalHereNode = node.GetNode("paypal-here");
            if (paypalHereNode != null)
            {
                PayPalHereDetails = new PayPalHereDetails(paypalHereNode);
            }
            var localPaymentNode = node.GetNode("local-payment");
            if (localPaymentNode != null)
            {
                LocalPaymentDetails = new LocalPaymentDetails(localPaymentNode);
            }
            var coinbaseNode = node.GetNode("coinbase-account");
            if (coinbaseNode != null)
            {
                CoinbaseDetails = new CoinbaseDetails(coinbaseNode);
            }
            var applePayNode = node.GetNode("apple-pay");
            if (applePayNode != null)
            {
                ApplePayDetails = new ApplePayDetails(applePayNode);
            }
            var androidPayNode = node.GetNode("android-pay-card");
            if (androidPayNode != null)
            {
                AndroidPayDetails = new AndroidPayDetails(androidPayNode);
            }
            var amexExpressCheckoutNode = node.GetNode("amex-express-checkout-card");
            if (amexExpressCheckoutNode != null)
            {
                AmexExpressCheckoutDetails = new AmexExpressCheckoutDetails(amexExpressCheckoutNode);
            }
            var venmoAccountNode = node.GetNode("venmo-account");
            if (venmoAccountNode != null)
            {
                VenmoAccountDetails = new VenmoAccountDetails(venmoAccountNode);
            }
            var usBankAccountNode = node.GetNode("us-bank-account");
            if (usBankAccountNode != null)
            {
                UsBankAccountDetails = new UsBankAccountDetails(usBankAccountNode);
            }
            // NEXT_MAJOR_VERSION Remove this class as legacy Ideal has been removed/disabled in the Braintree Gateway
            // DEPRECATED If you're looking to accept iDEAL as a payment method contact accounts@braintreepayments.com for a solution.
            var idealPaymentNode = node.GetNode("ideal-payment");
            if (idealPaymentNode != null)
            {
                IdealPaymentDetails = new IdealPaymentDetails(idealPaymentNode);
            }
            var visaCheckoutNode = node.GetNode("visa-checkout-card");
            if (visaCheckoutNode != null)
            {
                VisaCheckoutCardDetails = new VisaCheckoutCardDetails(visaCheckoutNode);
            }
            var masterpassNode = node.GetNode("masterpass-card");
            if (masterpassNode != null)
            {
                MasterpassCardDetails = new MasterpassCardDetails(masterpassNode);
            }
            var samsungPayNode = node.GetNode("samsung-pay-card");
            if (samsungPayNode != null)
            {
                SamsungPayCardDetails = new SamsungPayCardDetails(samsungPayNode);
            }

            var billingAddressNode = node.GetNode("billing");
            if (billingAddressNode != null)
            {
                BillingAddress = new Address(billingAddressNode);
            }
            var shippingAddressNode = node.GetNode("shipping");
            if (shippingAddressNode != null)
            {
                ShippingAddress = new Address(shippingAddressNode);
            }

            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");

            AddOns = new List<AddOn>();
            foreach (var addOnResponse in node.GetList("add-ons/add-on"))
            {
                AddOns.Add(new AddOn(addOnResponse));
            }
            Discounts = new List<Discount>();
            foreach (var discountResponse in node.GetList("discounts/discount"))
            {
                Discounts.Add(new Discount(discountResponse));
            }

            Disputes = new List<Dispute>();
            foreach (var dispute in node.GetList("disputes/dispute"))
            {
                Disputes.Add(new Dispute(dispute));
            }

            AuthorizationAdjustments = new List<AuthorizationAdjustment>();
            foreach (var authorizationAdjustment in node.GetList("authorization-adjustments/authorization-adjustment"))
            {
                AuthorizationAdjustments.Add(new AuthorizationAdjustment(authorizationAdjustment));
            }

            var riskDataNode = node.GetNode("risk-data");
            if (riskDataNode != null)
            {
                RiskData = new RiskData(riskDataNode);
            }

            var threeDSecureInfoNode = node.GetNode("three-d-secure-info");
            if (threeDSecureInfoNode != null && !threeDSecureInfoNode.IsEmpty())
            {
                ThreeDSecureInfo = new ThreeDSecureInfo(threeDSecureInfoNode);
            }

            var facilitatedDetailsNode = node.GetNode("facilitated-details");
            if (facilitatedDetailsNode != null && !facilitatedDetailsNode.IsEmpty())
            {
                FacilitatedDetails = new FacilitatedDetails(facilitatedDetailsNode);
            }

            var facilitatorDetailsNode = node.GetNode("facilitator-details");
            if (facilitatorDetailsNode != null && !facilitatorDetailsNode.IsEmpty())
            {
                FacilitatorDetails = new FacilitatorDetails(facilitatorDetailsNode);
            }

            DiscountAmount = node.GetDecimal("discount-amount");
            ShippingAmount = node.GetDecimal("shipping-amount");
            ShipsFromPostalCode = node.GetString("ships-from-postal-code");

            NetworkTransactionId = node.GetString("network-transaction-id");

            AuthorizationExpiresAt = node.GetDateTime("authorization-expires-at");

            RetrievalReferenceNumber = node.GetString("retrieval-reference-number");
        }

        /// <summary>
        /// Returns the current <see cref="CreditCard"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current <see cref="CreditCard"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the credit card used in the transaction is returned in the response.
        /// If the credit card record has been updated in the vault since the transaction occurred, this method can be used to
        /// retrieve the updated credit card information.  This is typically useful in situations where a transaction fails, for
        /// example when a credit card expires, and a new transaction needs to be submitted once the new credit card information
        /// has been submitted.
        /// </remarks>
        /// <example>
        /// The vault <see cref="CreditCard"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     CreditCard creditCard = transaction.GetVaultCreditCard();
        /// </code>
        /// </example>
        /// <example>
        /// Failed transactions can be resubmitted with updated <see cref="CreditCard"/> information:
        /// <code>
        ///     Transaction failedTransaction = gateway.Transaction.Find("transactionId");
        ///     CreditCard updatedCreditCard = transaction.GetVaultCreditCard();
        ///
        ///     TransactionRequest request = new TransactionRequest
        ///     {
        ///         Amount = failedTransaction.Amount,
        ///         PaymentMethodToken = updatedCreditCard.Token
        ///     };
        ///
        ///     Result&lt;Transaction&gt; result = gateway.Transaction.Sale(request);
        /// </code>
        /// </example>
        public virtual CreditCard GetVaultCreditCard()
        {
            if (CreditCard.Token == null) return null;

            return new CreditCardGateway(Gateway).Find(CreditCard.Token);
        }

        /// <summary>
        /// Returns the current <see cref="Customer"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current <see cref="Customer"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the customer associated with the transaction is returned in the response.
        /// If the customer record has been updated in the vault since the transaction occurred, this method can be used to
        /// retrieve the updated customer information.
        /// </remarks>
        /// <example>
        /// The vault <see cref="Customer"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Customer customer = transaction.GetVaultCustomer();
        /// </code>
        /// </example>
        public virtual Customer GetVaultCustomer()
        {
            if (CustomerDetails == null || CustomerDetails.Id == null) return null;

            return new CustomerGateway(Gateway).Find(CustomerDetails.Id);
        }

        /// <summary>
        /// Returns the current billing <see cref="Address"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current billing <see cref="Address"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the billing address associated with the transaction is returned in the response.
        /// If the billing address has been updated in the vault since the transaction occurred, this method can be used to
        /// retrieve the updated billing address.
        /// </remarks>
        /// <example>
        /// The vault billing <see cref="Address"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Address billingAddress = transaction.GetVaultBillingAddress();
        /// </code>
        /// </example>
        public virtual Address GetVaultBillingAddress()
        {
            if (BillingAddress.Id == null) return null;

            return new AddressGateway(Gateway).Find(CustomerDetails.Id, BillingAddress.Id);
        }

        /// <summary>
        /// Returns the current shipping <see cref="Address"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current shipping <see cref="Address"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the shipping address associated with the transaction is returned in the response.
        /// If the shipping address has been updated in the vault since the transaction occurred, this method can be used to
        /// retrieve the updated shipping address.
        /// </remarks>
        /// <example>
        /// The vault shipping <see cref="Address"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Address shippingAddress = transaction.GetVaultShippingAddress();
        /// </code>
        /// </example>
        public virtual Address GetVaultShippingAddress()
        {
            if (ShippingAddress.Id == null) return null;

            return new AddressGateway(Gateway).Find(CustomerDetails.Id, ShippingAddress.Id);
        }

        public bool IsDisbursed()
        {
          return DisbursementDetails.IsValid();
        }

        /// <summary>
        /// Returns the list of <see cref="TransactionLineItem"/>s associated with this transaction
        /// </summary>
        /// <returns>
        /// The list of <see cref="TransactionLineItem"/>s associated with this transaction
        /// </returns>
        /// <example>
        /// The list of <see cref="TransactionLineItem"/>s can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     List&lt;TransactionLineItem&gt; lineItems = transaction.GetLineItems();
        /// </code>
        /// </example>
        public virtual List<TransactionLineItem> GetLineItems()
        {
            return Gateway.TransactionLineItem.FindAll(Id);
        }
    }
}
