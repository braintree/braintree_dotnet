# Changelog

## 5.33.0
- Add `AccountInformationInquiry` to:
  - `Customer.create` and `Customer.update`
  - `CreditCard.create` and `CreditCard.update`
  - `PaymentMethod.create` and `PaymentMethod.update`
  - `CreditCardVerification.create`
- Enhancements to PayPal customer recommendations
  - Create a session and get recommendations in one call
  - Hash customer email and phone number
- Add `business`, `consumer`, `corporate`, and `purchase` from the bin data to credit card responses

## 5.32.0
- Remove marketplace features
- Add support for capturing payment facilitator and sub-merchant details with transactions 

## 5.31.0
- Add support for `PayPalPaymentResource` requests
- Add `prepaid_reloadable` from the bin data to credit card responses

## 5.30.0
- Add support for creating and updating PayPal customer session
- Add support for getting PayPal customer recommendations

## 5.29.0
- Add recipient/contact info: `recipient_email`and `recipient_phone` to `Transaction` 

## 5.28.0
- Add `fail_on_duplicate_payment_method_for_customer` option to 
  - `ClientToken`
  - `PaymentMethod`
  - `CreditCard`
- Fix billing address bug for credit card verification.
- Add `blik_aliases` to LocalPaymentCompleted webhook and LocalPaymentDetail
- Deprecate `SamsungPayCard`

## 5.27.0 
- Add `PayerName`, `Bic` and `IbanLastChars` to LocalPaymentCompleted webhook
- Add `edit_paypal_vault_id` to `PayPalAccount`
- Add `AniFirstNameResponseCode` and `AniLastNameResponseCode` to `CreditCardVerification`
- Add `ShippingTaxAmount` to `Transaction` and `TransactionRequest`
- Add `networkTokenizationAttributes` to `TransactionCreditCardRequest`
- Add `NetworkTokenizationAttributesRequest` class
- Add validation error codes:
  - `CREDIT_CARD_NETWORK_TOKENIZATION_ATTRIBUTE_CRYPTOGRAM_IS_REQUIRED`

## 5.26.0
- Add attributes to `ThreeDSecureLookupRequest`
  - `MerchantInitiatedRequestType`
  - `PriorAuthenticationId`
  - `MerchantOnRecordName`
  - `PriorAuthenticationDetails`
- Add `ForeignRetailer` to `Transaction` and `TransactionRequest`
- Add `InternationalPhone` to `Address` and `Customer`
- Add `fundingSourceDescription` to `PayPalAccount`
- Add missing `AndroidPayCard` error code
- Add `REFUND_FAILED` to `WebhookKind`
- Add `FinalCapture` to Transaction `TransactionRequest`
- Deprecate `paypal_tracking_id` in favor of `paypal_tracker_id` in PackageDetails

## 5.25.0
- Add `domains` parameter support to `ClientTokenRequest`

## 5.24.0
- Add `UNDER_REVIEW` to `DisputeStatus`
- Add `DISPUTE_UNDER_REVIEW` to `WebhookKind`
- Add `DebitNetwork` to `Transaction`
- Add `TransactionDebitNetwork` enum to `Transaction`
- Add `DebitNetwork` to `TransactionSearchRequest` to search by debitNetworks
- Add `ProcessDebitAsCredit` to `TransactionOptionsCreditCardRequest`

## 5.23.0
- Add `PackageTracking` and `PackageTrackingAsync` methods to `TransactionGateway` to make request to add tracking information to transactions
- Add `PackageDetails` and `PackageTrackingRequest` classes
- Add `UpcCode`, `UpcType`, `ImageUrl` to `TransactionLineItem` and `TransactionLineItemRequest`

## 5.22.0
- Deprecate `threeDSecureToken` in favor of `threeDSecureAuthenticationId`
- Deprecate `VenmoSdkSession`, `VenmoSdkPaymentMethodCode`, and `IsVenmoSdk`
- Add `PICKUP_IN_STORE` to `ShippingMethod` enum
- Add `ExternalVault` and `RiskData` to `CreditCardVerificationRequest`

## 5.21.0
- Add `DateOfBirth` and `CountryCode` to `IndustryDataRequest`
- Add `BillingAddress` and `ShippingAddress` to `VenmoProfileData`
- Add `VerificationAddOns` and `AdditionalProcessorResponse` to `UsBankAccountVerification`
- Add `MetaCheckoutCard` and `MetaCheckoutToken` to payment method
- Add `MetaCheckoutCardDetails` and `MetaCheckoutTokenDetails` to Transaction

## 5.20.0
- Add `SUBSCRIPTIONBILLINGSKIPPED` webhook notification support
- Add `implicitlyVaultedPaymentMethodToken` and `implicitlyVaultedPaymentMethodGlobalId` to `LocalPaymentDetails`

## 5.19.0
- Add `MerchantTokenIdentifier`, `SourceCardLast4` to `ApplePayCard` and `ApplePayDetails`
- Add `ThreeDSecureAuthenticationId` to `CustomerRequest` and `PaymentMethodRequest`
- Add `processing_overrides` to `Transaction.sale` options
- Add industry data fields to `TransactionRequest`
    - `ArrivalDate`
    - `TicketIssuerAddress`
- Remove `byte[]` to `String` content conversion for requests containing files in `BraintreeService`

## 5.18.0
- Add `MerchantAdviceCode` and `MerchantAdviceCodeText` to `Transaction`
- Add `cardAddChallengeRequested` to `ThreeDSecureLookupRequest`
- Add `RetryIds` and `RetriedTransactionId` to `Transaction`

## 5.17.0
- Add subscriptions for SEPA direct debit account
- Add device data fields to `ThreeDSecureLookupRequest`
  - `BrowserAcceptHeader`
  - `BrowserColorDepth`
  - `BrowserJavaEnabled`
  - `BrowserJavascriptEnabled`
  - `BrowserLanguage`
  - `BrowserScreenHeight`
  - `BrowserScreenWidth`
  - `BrowserTimeZone`
  - `DeviceChannel`
  - `IpAddress`
  - `UserAgent`

## 5.16.0
- Add `IntendedTransactionSource` to `CreditCardVerificationRequest`
- Add `PaymentMethodNonce` to `CreditCardVerificationRequest`
- Add `ThreeDSecureAuthenticationID` to `CreditCardVerificationRequest`
- Add `ThreeDSecurePassThru` to `CreditCardVerificationRequest`
- Add support for Plan creation/updating with `AddOns` and `Discounts`

## 5.15.0
- Bump Newtonsoft.Json from 9.0.1 to 13.0.1
- Add `PreDisputeProgram` to `Dispute` and `DisputeSearchRequest`
- Add `AUTO_ACCEPTED` to `DisputeStatus` 
- Add `DISPUTE_AUTO_ACCEPTED` to `WebhookKind`
- Add `EXCESSIVE_RETRY` to `GatewayRejectionReason`
- Add `SepaDirectDebitAccount` payment method
- Add `SepaDirectDebitAccountDetails` and `SepaDirectDebitReturnCode` to transaction object
- Add `SepaDebitPayPalV2OrderId` to transaction search request object
- Add `MandateType` enum to payment method nonce details object
- Add `SEPA_DIRECT_DEBIT_ACCOUNT` to payment instrument type
- Add SEPA Direct Debit specific error codes
- Add SEPA Direct Debit array to customer object
- Add `PaymentMethodNonceDetailsSepaDirectDebit` to payment method nonce details
- Add `RequestedExemptionType` to `ThreeDSecureLookupRequest`

## 5.14.0
- Deprecate `ChargebackProtectionLevel` and add `ProtectionLevel` to `Dispute` and `DisputeSearch`
- Add a check for path canonicalization in request URI creation

## 5.13.0
- Add `AchReturnResponsesCreatedAt` to `TransactionSearchRequest` to search for specific date for transaction that have any reason codes.
- Add `ReasonCode` to `TransactionSearchRequest` to search for transactions that have return reason codes.
- Add `TransactionSearchRequest.ANY_REASON_CODE` to `TransactionSearchRequest` for reason code search that matches all reason codes.
- Add `LiabilityShift` to `RiskData`
- Add ExchangeRateQuote API

## 5.12.0
* Add `AchReturnCode` to transaction object
* Add `Retried` to transaction object

## 5.11.0
- Add `PaymentMethodCustomerDataUpdated` webhook notification support

## 5.10.0
- Add plan create/update/find API endpoint
- Add `TransactionReview` webhook notification support

## 5.9.0
- Add `LocalPaymentExpired` and `LocalPaymentFunded` webhook notification support

## 5.8.0
- Add error code `TRANSACTION_TAX_AMOUNT_IS_REQUIRED_FOR_AIB_SWEDISH` for attribute `tax_amount` in `transaction` key for AIB:Domestic Transactions in Sweden
- Add `ExchangeRateQuoteId` to `TransactionRequest`
- Add `EXCHANGE_RATE_QUOTE_ID_IS_TOO_LONG` to `ValidationErrorCode`
- Add the following fields to `AndroidPayCard` and `ApplePayCard`:
  - `Commercial`
  - `Debit`
  - `DurbinRegulated`
  - `Healthcare`
  - `Payroll`
  - `Prepaid`
  - `ProductId`
  - `CountryOfIssuance`
  - `IssuingBank`
- Add the following fields to `PaypalDetails`:
  - `TaxId`
  - `TaxIdType`

## 5.7.0
- Add `PaymentReaderCardDetails` to `TransactionCreditCardRequest`
- Add `SkipAdvancedFraudChecking` to:
  - `Customer.create` and `Customer.update`
  - `CreditCard.create` and `CreditCard.update`
  - `PaymentMethod.create` and `PaymentMethod.update`
- Add `DataOnlyRequested` to `ThreeDSecureLookupRequest`
- Add `ChargebackProtectionLevel` to `DisputeSearch`

## 5.6.0
- Add `PayPalMessages` to `Dispute`
- Add `TaxIdentifiers` to `CustomerRequest`
- Add webhook sample for `GrantedPaymentMethodRevoked`

## 5.5.0
- Add `StoreId` and `StoreIds` to `TransactionSearchRequest`
- Add support for `LocalPaymentReversed` webhook notifications
- Add `Transaction.AdjustAuthorization` method to support for multiple authorizations for a single transaction
- Add `MerchantAccountId` to `TransactionRefundRequest`
- Add `PhoneNumber` to `Address` (thanks @glennsdavis!)

## 5.4.0
- Add missing `ExpirationMonth`, `ExpirationYear`, and `IsNetworkTokenized` fields in `PaymentMethodNonceDetails` (thanks @glennsdavis for `ExpirationMonth` and `ExpirationYear`!)
- Add `ThreeDSecureAuthenticationInfo` and `ThreeDSecureLookupInfo` classes
- Add `AcsTransactionId`, `ParesStatus`, `ThreeDSecureTransactionId`, `Lookup`, and `Authentication` to `ThreeDSecureInfo`
- Add `DecisionReasons` and `TransactionRiskScore` fields to `RiskData`
- Add overload on `SubmitForSettlementAsync` method to include `TransactionRequest` (thanks @edtyl3r!)

## 5.3.0
- Add `scaExemption` to `Transaction`
- Deprecate `DeviceSessionId` and `FraudMerchantId` in the `CreditCardRequest` and `TransactionRequest` classes
- Add `currencyIsoCode` field to `TransactionRequest`
- Add `verificationCurrencyIsoCode` field to `CreditCardOptionsRequest` and `PaymentMethodOptionsRequest`
- Add validation error codes:
  - `CREDIT_CARD_OPTIONS_VERIFICATION_INVALID_PRESENTMENT_CURRENCY`
  - `TRANSACTION_INVALID_PRESENTMENT_CURRENCY`
- Add `Installments` to `TransactionRequest`
- Add `Count` to `InstallmentRequest`
- Add `Installments` and `RefundedInstallments` to `Transaction`
- Add `Adjustment` to `Installment`

## 5.2.0
- Add `AcquirerReferenceNumber` to `Transaction`
- Add `BillingAgreementId` to `PayPalDetails`
- Add `extensions` to `GraphQLResponse`
- Deprecate `Recurring` in TransactionRequest

## 5.1.0
- Add `InstallmentCount` to `DisputeTransaction` on dispute webhooks
- Add `ImplicitlyVaultedPaymentMethodToken` and `ImplicitlyVaultedPaymentMethodGlobalId` to `PayPalDetails`

## 5.0.0
- Add `RequestTimeoutException` and `GatewayTimeoutException`
- Add `RISK_THRESHOLD` to GatewayRejectionReason constants
- Add `processedWithNetworkToken` to `Transaction`
- Add `isNetworkTokenized` to `CreditCard`
Breaking changes:
- Drop support for .NET Core versions less than 2.1 (v1.0, v1.3, v2.0, etc)
- Drop .NET Standard 1.3 from `TargetFrameworks`
- Remove deprecated `TransparentRedirect`, `iDEAL`, `Coinbase`, and `IbanAccount` classes
- Remove deprecated SEPA mandate and Europe Bank Account error codes, test payment method nonces, and payment intrument types
- Remove `ForwardedComments` from `Dispute`
- Remove `PayPalVaultWithoutUpgrade` from `PaymentMethodRequest`
- Remove deprecated validation error codes:
  - `SEPA_BANK_ACCOUNT_ACCOUNT_HOLDER_NAME_IS_REQUIRED`
  - `SEPA_BANK_ACCOUNT_BIC_IS_REQUIRED`
  - `SEPA_BANK_ACCOUNT_IBAN_IS_REQUIRED`
  - `SEPA_MANDATE_ACCOUNT_HOLDER_NAME_IS_REQUIRED`
  - `SEPA_MANDATE_BIC_INVALID_CHARACTER`
  - `SEPA_MANDATE_BIC_IS_REQUIRED`
  - `SEPA_MANDATE_BIC_LENGTH_IS_INVALID`
  - `SEPA_MANDATE_BIC_UNSUPPORTED_COUNTRY`
  - `SEPA_MANDATE_BILLING_ADDRESS_CONFLICT`
  - `SEPA_MANDATE_BILLING_ADDRESS_ID_IS_INVALID`
  - `SEPA_MANDATE_IBAN_INVALID_CHARACTER`
  - `SEPA_MANDATE_IBAN_INVALID_FORMAT`
  - `SEPA_MANDATE_IBAN_IS_REQUIRED`
  - `SEPA_MANDATE_IBAN_UNSUPPORTED_COUNTRY`
  - `SEPA_MANDATE_LOCALE_IS_UNSUPPORTED`
  - `SEPA_MANDATE_TYPE_IS_REQUIRED`
  - `TRANSACTION_AMOUNT_DOES_NOT_MATCH_IDEAL_PAYMENT_AMOUNT`
  - `TRANSACTION_IDEAL_PAYMENT_NOT_COMPLETE`
  - `TRANSACTION_IDEAL_PAYMENTS_CANNOT_BE_VAULTED`
  - `TRANSACTION_MERCHANT_ACCOUNT_DOES_NOT_MATCH_IDEAL_PAYMENT_MERCHANT_ACCOUNT`
  - `TRANSACTION_ORDER_ID_DOES_NOT_MATCH_IDEAL_PAYMENT_ORDER_ID`
  - `TRANSACTION_ORDER_ID_IS_REQUIRED_WITH_IDEAL_PAYMENT`
- Remove deprecated webhook kinds:
  - `GRANTED_PAYMENT_INSTRUMENT_UPDATE`
  - `IDEAL_PAYMENT_COMPLETE`
  - `IDEAL_PAYMENT_FAILED`
- Rename `DownForMaintenanceException` to `ServiceUnavailableException`
- Transaction searches and Transaction Line Items FindAll calls throw `UnexpectedException` instead of `DownForMaintenance` when search response yields unexpected results
- Remove `UseStaticHttpClient` configuration option for .NET Core integrations
- Remove `MasterpassCard` payment method
- Remove `AmexExpressCheckoutCard` payment method
- Bump API version to support declined refund objects
- Convert custom `Enumeration` subclasses to C# `Enum` types (fixes #86):
  - `CreditCardCardType` (additionally, type MASTER_CARD was renamed to MASTERCARD)
  - `CreditCardCommercial`
  - `CreditCardCustomerLocation`
  - `CreditCardDebit`
  - `CreditCardDurbinRegulated`
  - `CreditCardHealthcare`
  - `CreditCardPayroll`
  - `CreditCardPrepaid`
  - `DisbursementType`
  - `DisputeKind`
  - `DisputeReason`
  - `DisputeStatus`
  - `DocumentUploadKind`
  - `FundingDestination`
  - `IndustryDataAdditionalChargeKind`
  - `MerchantAccountStatus`
  - `PaymentInstrumentType`
  - `PlanDurationUnit`
  - `ProcessorResponseType`
  - `ShippingMethod`
  - `SubscriptionDurationUnit`
  - `SubscriptionSource`
  - `SubscriptionStatus`
  - `TransactionCreatedUsing`
  - `TransactionEscrowStatus`
  - `TransactionGatewayRejectionReason`
  - `TransactionIndustryType`
  - `TransactionLineItemKind`
  - `TransactionSource`
  - `TransactionStatus`
  - `TransactionType`
  - `UsBankAccountVerificationMethod`
  - `UsBankAccountVerificationStatus`
  - `VerificationStatus`
  - `WebhookKind`

## 4.18.0
- Add `ThreeDSecurePassThru` to `CreditCard.create()`, `CreditCard.update()`, `Customer.create()`, `Customer.update()`, `PaymentMethod.create()` and `PaymentMethod.update()`
- Add ThreeDSecure test payment method nonces
- Add test `AuthenticationId`s
- Add `DISPUTE_ACCEPTED`, `DISPUTE_DISPUTED`, and `DISPUTE_EXPIRED` webhook constants
- Add Authentication Insight to `PaymentMethodNonce.Create`
- Add `DOCUMENT_UPLOAD_FILE_IS_EMPTY` error code
- Add `RetrievalReferenceNumber` to `Transaction`
- Remove `Expect:100-Continue` header from requests to stop sporadic failed requests.
- Add `FromNonceAsync` method to `CreditCardGateway` (#97 thanks @ronnieoverby)
- Add `NetworkTransactionId` to `CreditCardVerification`
- Fix blocking in HttpService for .NET Core async methods (#100 thanks @roblascelles)
- Add `ProductSku` to `TransactionRequest`
- Add `PhoneNumber` and `ShippingMethod` to `AddressRequest`
- Add `CustomerDeviceId`, `CustomerLocationZip`, and `CustomerTenure` to `RiskDataRequest`
- Add validation error codes:
  - `TRANSACTION_PRODUCT_SKU_IS_INVALID`
  - `TRANSACTION_SHIPPING_PHONE_NUMBER_IS_INVALID`
  - `TRANSACTION_BILLING_PHONE_NUMBER_IS_INVALID`
  - `RISK_DATA_CUSTOMER_BROWSER_IS_TOO_LONG`
  - `RISK_DATA_CUSTOMER_DEVICE_ID_IS_TOO_LONG`
  - `RISK_DATA_CUSTOMER_LOCATION_ZIP_INVALID_CHARACTERS`
  - `RISK_DATA_CUSTOMER_LOCATION_ZIP_IS_INVALID`
  - `RISK_DATA_CUSTOMER_LOCATION_ZIP_IS_TOO_LONG`
  - `RISK_DATA_CUSTOMER_TENURE_IS_TOO_LONG`

## 4.17.0
- Add `RefundAuthHardDeclined` and `RefundAuthSoftDeclined` to validation errors
- Add `PROCESSOR_DOES_NOT_SUPPORT_MOTO_FOR_CARD_TYPE` to validation errors
- Add `isNetworkTokenized` to `AndroidPayCard` and `AndroidPayDetails`
- Add GraphQL ID to `CreditCardVerification`, `Customer`, `Dispute`, and `Transaction`
- Fix issue where GraphQL errors from transaction fee reports could not be parsed
- Add `ThreeDSecureAuthenticationId` to `ThreeDSecureInfo`
- Add `ThreeDSecureAuthenticationId` support to Transaction.Sale

## 4.16.0
- Add `ProcessorComments` to Disputes
- Deprecate `ForwardedComments`
- Add `AMOUNT_NOT_SUPPORTED_BY_PROCESSOR` to validation errors

## 4.15.0
- Add `ThreeDSecureLookup` (beta)

## 4.14.0
- Add `GraphQLClient` instance to `BraintreeGateway` class

## 4.13.0
- Add `PayPalHereDetails` to transactions
- Add `CaptureId` to `LocalPaymentDetails`
- Add `RefundId` to `LocalPaymentDetails`
- Add `DebugId` to `LocalPaymentDetails`
- Add `RefundFromTransactionFeeAmount` to `LocalPaymentDetails`
- Add `RefundFromTransactionFeeCurrencyIsoCode` to `LocalPaymentDetails`
- Add `TransactionFeeAmount` to `LocalPaymentDetails`
- Add `TransactionFeeCurrencyIsoCode` to `LocalPaymentDetails`
- Add `Cavv`, `Xid`, `DsTransactionId`, `EciFlag`, and `ThreeDSecureVersion` to `ThreeDSecureInfo`
- Add `ThreeDSecureVersion`, `AuthenticationResponse`, `DirectoryResponse`, `CavvAlgorithm`, and `DsTransactionId` to 3DS pass thru fields
- Add `PayerInfo` to `PaymentMethodNonceDetails` class
- Add `NetworkResponseCode` and `NetworkResponseText` fields to `Transaction` and `CreditCardVerification`
- Add `ThreeDSecureInfo` to `CreditCardVerification`

## 4.12.0

- Add `token_issuance` gateway rejected status to support Venmo transactions
- Add `payment_method_nonce` field to `LocalPaymentCompleted` webhook
- Add `transaction` field to `LocalPaymentCompleted` webhook
- Add `LocalPaymentDetails` to transactions
- Add `RoomTax` field to `IndustryDataRequest`
- Add `NoShow` field to `IndustryDataRequest`
- Add `AdvancedDeposit` field to `IndustryDataRequest`
- Add `FireSafe` field to `IndustryDataRequest`
- Add `PropertyPhone` field to `IndustryDataRequest`
- Add `AdditionalChargeRequest` class and add `AdditionalCharges` field to `IndustryDataRequest`
- Add `PostalCodeIsRequiredForCardBrandAndProcessor` to validation errors
- Resolve an issue when building the project in some configurations
- Add `RefundFromTransactionFeeAmount` field to `PayPalDetails`
- Add `RefundFromTransactionFeeCurrencyIsoCode` field to `PayPalDetails`
- Add `RevokedAt` field to `PayPalAccount`
- Add support for `PAYMENT_METHOD_REVOKED_BY_CUSTOMER` webhook

## 4.11.0

- Add `AccountType` field to `Transaction`, `CreditCard`, `PaymentMethod`, and `CreditCardVerification`
- Add `RefundFromTransactionFeeAmount` field to `PayPalDetails`
- Add `RefundFromTransactionFeeCurrencyIsoCode` field to `PayPalDetails`
- Add `RevokedAt` field to `PayPalAccount`
- Add support for `PAYMENT_METHOD_REVOKED_BY_CUSTOMER` webhook

## 4.10.1

- Resolve an issue with `PackageReleaseNotes` in `Braintree.csproj` that prevented building the project

## 4.10.0

- Deprecate `GRANTED_PAYMENT_INSTRUMENT_UPDATE` and add `GRANTOR_UPDATED_GRANTED_PAYMENT_METHOD` and `RECIPIENT_UPDATED_GRANTED_PAYMENT_METHOD`
- Add `Prepaid` field to `ApplePayDetails`
- Add `Healthcare` field to `ApplePayDetails`
- Add `Debit` field to `ApplePayDetails`
- Add `DurbinRegulated` field to `ApplePayDetails`
- Add `Commercial` field to `ApplePayDetails`
- Add `Payroll` field to `ApplePayDetails`
- Add `IssuingBank` field to `ApplePayDetails`
- Add `CountryOfIssuance` field to `ApplePayDetails`
- Add `ProductId` field to `ApplePayDetails`
- Add `Bin` field to `ApplePayDetails`
- Add `GlobalId` field to `ApplePayDetails`
- Add `Prepaid` field to `AndroidPayDetails`
- Add `Healthcare` field to `AndroidPayDetails`
- Add `Debit` field to `AndroidPayDetails`
- Add `DurbinRegulated` field to `AndroidPayDetails`
- Add `Commercial` field to `AndroidPayDetails`
- Add `Payroll` field to `AndroidPayDetails`
- Add `IssuingBank` field to `AndroidPayDetails`
- Add `CountryOfIssuance` field to `AndroidPayDetails`
- Add `ProductId` field to `AndroidPayDetails`
- Add `GlobalId` field to `AndroidPayDetails`
- Cross compile for netstandard 2.0
- ResourceCollection<T> now implements IEnumerable<T>

## 4.9.0

- Add Error indicating pdf uploads too long for dispute evidence.
- Add `bin` field to `PaymentMethodNonceDetails`
- Fix creating transaction from local payment webhook data
- Add `GrantedPaymentMethodRevoked` webhook response objects
- Add `Username` and `VenmoUserId` to `PaymentMethodNonceDetails`.

## 4.8.0

- Allow PayPal payment ID and payer ID to be passed during transaction create
- Add `travel_flight` support to industry-specific data
- Add `fraudServiceProvider` field to `RiskData`
- Add `UseStaticHttpClient` to configuration to reuse the same http client instance in each request

## 4.7.0

- Add `AuthorizationExpiresAt` to `Transaction`
- Add `ProcessorResponseType` to `Transaction`, `AuthorizationAdjustment`, and `CreditCardVerification`

## 4.6.0

- Allow environemnt parsing to be case insensitive (#75)
- Add `LastFour` to `PaymentMethodNonceDetails`
- Add subscription charged unsuccessfully sample webhook to webhook testing gateway
- Fix dispute results in transactions not showing the correct status sometimes
- Add `network_transaction_id` as new field on subfield transaction response.
- Add support for `ExternalVaultRequest` for TransactionRequest
- Add support for `LocalPaymentCompleted` webhook notifications.

## 4.5.0

- Add Samsung Pay support
- Add `processor_response_code` and `processor_response_text` to authorization adjustments subfield in transaction response.

## 4.4.0

- Add `MerchantId` to `ConnectedMerchantStatusTransitioned` and `ConnectedMerchantPayPalStatusChanged`, and `OauthApplicationClientId` to OAuthAccessRevocation webhooks
- Fix webhook testing sample xml for dispute webhooks to include `amount-won` and `amount-disputed`

## 4.3.0

- Fix CreditCard not correctly identifying Union Pay card type
- Allow payee ID to be passed in options params for transaction create

## 4.2.0

- Add support for US Bank Account verifications API

## 4.1.1

- Add dispute error code DISPUTE_VALID_EVIDENCE_REQUIRED_TO_FINALIZE
- Fix incorrectly named FileEvidenceRequest.DocumentId parameter

## 4.1.0

- Add support for VCR compelling evidence dispute representment

## 4.0.0

### Backwards incompatible changes

- Remove non-CLS compliant `generate` and `generateAsync` methods from `ClientTokenGateway`. Use `Generate` or `GenerateAsync`.
- Add `SubscriptionDetails` class and change `Transaction.Subscription` property to `Transaction.SubscriptionDetails`.
- Add `CustomerDetails` class and change `Transaction.Customer` property to `Transaction.CustomerDetails`.
- Remove `RefundId` from `Transaction`. Use `RefundIds` instead.
- Remove `TransparentRedirectURLForCreate`, `TransparentRedirectURLForUpdate`, and `ConfirmTransparentRedirect` from `CreditCardGateway`, `CustomerGateway`, and `TransactionGateway`. Use `gateway.TransparentRedirect.Url` or `gateway.TransparentRedirect.Confirm()` instead.
- Remove `CUSTOMER_ID_IS_INVAILD`, `TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_MUST_BE_GREATER_THAN_ZERO`, and `TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_MUST_BE_GREATER_THAN_ZERO`, and `TRANSACTION_MERCHANT_ACCOUNT_NAME_IS_INVALID`. Use `CUSTOMER_ID_IS_INVALID`, `TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_CANNOT_BE_NEGATIVE`, `TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_CANNOT_BE_NEGATIVE`, or `TRANSACTION_MERCHANT_ACCOUNT_ID_IS_INVALID` instead.
- If an `AccessToken` and `Environment` are specified in `Configuration`, an exception will be thrown if the environment does not match the access token's environment.
- For `RangeNode` decimal values passed to `SearchCriteria`, use `.ToString(InvariantCulture)` to prevent localization issues (https://github.com/braintree/braintree_dotnet/issues/68).
- Change `OAuthGateway.ComputeSignature` to be an internal method.
- Migrate the xproj build files to MSBuild csproj build files and update Dockerfile images for support.

## 3.14.0

- Add `PayerId` accessor in `PayPalAccount`
- Add support for searching disputes by `effectiveDate`, `disbursementDate`, and `customerId`

## 3.13.0

- Add support for `association_filter_id` in `Customer#find`
- Add support for `OAUTH_ACCESS_REVOKED` in `WebhookNotification`s
- Add support for US Bank Account verifications via `PaymentMethod#create`, `PaymentMethod#update`, and `Transaction#create`
- Add support for US Bank Account verification search

## 3.12.0

- Add support for `TaxAmount` field on transaction LineItems
- Deprecate TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_MUST_BE_GREATER_THAN_ZERO error in favor of TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_CANNOT_BE_NEGATIVE.
- Deprecate TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_MUST_BE_GREATER_THAN_ZERO error in favor of TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_CANNOT_BE_NEGATIVE.
- Fix bug in `TransactionLineItemRequest` to use decimal, rather than double, for currency amount field. This is a backwards incompatible bug fix.
- Add `SourceMerchantId` property to `WebhookNotification`s if present
- Add support for `profile_id` in Transaction#create options for VenmoAccounts

## 3.11.0

- Change namespacing of Rakefile, so that Mono tests are run with `rake mono:test:all` and Core tests are run with `rake core:test:all`
- Add support for Level 3 summary parameters `shippingAmount`, `discountAmount`, and `shipsFromPostalCode`
- Add support for transaction line items
- Add support for tagged evidence in `DisputeGateway#AddTextEvidence` and `AddTextEvidenceAsync` (Beta release)

## 3.10.1

- Deprecate `OAuthGateway::ComputeSignature`
- Fix spec to expect PayPal transactions to move to settling rather than settled
- Fix absolute Url being passed to `SetWebProxy` in .NET Core

## 3.10.0

- Add GrantedPaymentInstrumentUpdate webhook support
- Add loginOnly parameter to OAuth connect URL
- Add ability to create a transaction from a shared nonce
- Fix spec to expect PayPal transaction to settle immediately after successful capture
- Add `options` -> `paypal` -> `shipping` for creating & updating customers as well as creating payment methods
- Add `ImageUrl` to `ApplePayDetails`
- Add `BinData` to `PaymentMethodNonce`

## 3.9.0

- Add `IDEAL_PAYMENT` to `PaymentInstrumentType`
- Improve async for .NET 4.5 (Thanks, @onlyann)
- Add additional properties to PaymentMethodNonce
- Add AuthorizationAdjustment class and `AuthorizationAdjustments` to Transaction
- Coinbase is no longer a supported payment method. `PAYMENT_METHOD_NO_LONGER_SUPPORTED` will be returned for Coinbase operations.
- Add facilitated transaction information to Transaction if present
- Add `SubmitForSettlement` for `SubscriptionGateway.RetryCharge`
- Add `options` -> `paypal` -> `description` for creating and updating subscriptions
- Add `Dispute` API
- Add `DocumentUpload` API
- Add `Bin` to `ApplePayCard`
- Add `deviceDataCaptured` field to `RiskData`
- Fix bug where `DeviceData` was not included in `CustomerRequest`
- Fix bug where `VisaCheckoutCards` and `MasterpassCards` were not included in `Customer`
- Add support for upgrading a PayPal future payment refresh token to a billing agreement

## 3.8.0

- Add support for additional PayPal options when vaulting a PayPal Order

## 3.7.0

- Add Visa Checkout support
- Add ConnectedMerchantStatusTransitioned and ConnectedMerchantPayPalStatusChanged Auth webhooks
- Parse access tokens properly
- Remove unused dependencies

## 3.6.0

- Add async support (thanks, @ejohnsonTKTNET and @ebonilla40!)
- Update Accept-Encoding and User-Agent headers in dotnet core
- Add stricter XML processing

## 3.5.0

- Stop sending account_description field from us bank accounts
- Add multi-currency support for OAuth Onboarded Merchants
- Add functionality to list all merchant accounts for a merchant with `MerchantAccount.all`

## 3.4.0

- Add payer_status accessor to paypal_details object
- Add option `skip_advanced_fraud_check` for transaction flows

## 3.3.0

- Allow custom verification amount in `PaymentMethodOptionsRequest`
- Allow `PaymentMethod.find(token)` to return `UsBankAccount`

## 3.2.1

- Make Newtonsoft.json a test-only dependency

## 3.2.0

- Change compatibility from .NET Standard 1.6 to 1.3 (Thanks, @hughbe)
- Add `UsBankAccount` payment method
- Throw `ConfigurationException` for missing credentials

## 3.1.0

- Add createdAt to SubscriptionSearch
- Add cannot clone marketplace transaction error
- Add PlanId to subscription status event
- Add grant and revoke APIs to PaymentMethodGateway
- Support TLS v1.2
- Support netstandard 1.3
- Throw configuration exception for empty strings

## 3.0.1

- Change compatibility with .NET Core 1.0 to .NET Standard 1.6 (Thanks, @thoughtentity!)
- Change to dotnet pack deployment strategy (Thanks, @devscott!)

## 3.0.0

- Remove compatibility with .NET 4.5.1 and below
- Add compatibility with .NET Core 1.0

## 2.65.0

- Add validation error for verifications with sub-merchants
- Expose credit card product ID
- Add currency iso code
- Update ValidationErrors.DeepAll() to be virtual

## 2.64.0

- Accept WebProxy in Configuration
- Set PaymentInstrumentType to VenmoAccount for Pay with Venmo transactions

## 2.63.0

- Add support for passing risk data on Customer and Transaction requests
- Add support for passing `transaction_source` to set MOTO or recurring ECI flag

## 2.62.0

- Set DefaultPaymentMethod when updating Customer
- Allow passing OAuth scopes to merchant create

## 2.61.0

- Add OrderId to refund
- Add 3DS Pass thru support
- Expose ids in resource collections

## 2.60.0

- Add method of revoking OAuth access tokens

## 2.59.0

- Add Transaction `UpdateDetails`
- Support for Too Many Request response code
- Add support for custom HttpWebRequest in configuration

## 2.58.0

- Add landing_page param to ConnectUrlRequest

## 2.57.0

- Allow Coinbase account to be updated
- Add support to pass currencies to merchant create
- Support multiple partial settlements
- Add IsInvalid error code for addresses
- Add establishedOn to partner business data

## 2.56.0

- Add timeout attribute to Configuration
- Add shared vault parameters

## 2.55.0

- Add AccountUpdaterDailyReport webhook parsing

## 2.54.0

- Add Strong Naming support
- Fix proxy setup to only set proxy if it is set in configuration. Thanks @chrisjdiver

## 2.53.0

- Support setting environment with just a string
- Add support for easy mocking during testing (thanks @richardlawley)
- Add new error codes

## 2.52.0

- Add VenmoAccount payment method

## 2.51.0

- Add `CHECK` webhook kind

## 2.50.0

- Add support for partial settlement transactions
- Add customer_id to payment methods

## 2.49.0

- Add sourceDescription attribute to Android Pay and Apple Pay
- Add new Android Pay test nonces

## 2.48.0

- Add BillingAgreementId to PayPalAccount
- Add support for amex rewards transactions

## 2.47.0

- Add new test payment method nonces
- Add methods to change transaction settlement status in sandbox

## 2.46.0

- Fix issue with Dispute Status of Open returning Unrecognized
- Add Description parameter for PayPal email receipts to be passed on Transaction.Sale request
- Add PayPal transaction fees and transaction fee currency in Transaction.Sale response

## 2.45.0

- Add Transaction Details to Dispute webhook notifications

## 2.44.0

- Add support for OAuth based authentication

## 2.43.0

- Add support for Android Pay

## 2.42.0

- Validate webhook challenge payload

## 2.41.0

- Add 3DS info the server side

## 2.40.0

- Add 3D Secure transaction fields
- Add ability to create nonce from vaulted payment methods

## 2.39.0

- Add Coinbase support
- Surface Apple Pay payment instrument name
- Support making 3D Secure required
- Surface subscription status events

## 2.38.0

- Allow PayPal fields in transaction.options.paypal
- Add error code constants

## 2.37.0

- Add risk_data to Transaction and Verification with Kount decision and id
- Add verification_amount an option when creating a credit card
- Add TravelCruise industry type to Transaction
- Add room_rate to Lodging industry type
- Add CreditCard#verification as the latest verification on that credit card
- Add ApplePay support to all endpoints that may return ApplePayCard objects
- Add prefix to sample Webhook to simulate webhook query params

## 2.36.0

- Allow descriptor to be passed in Funding Details options params for Merchant Account create and update.

## 2.35.0

- Add additionalProcessorResponse to transaction

## 2.34.1

- Allow payee_email to be passed in options params for Transaction create

## 2.34.0

- Added paypal specific fields to transaction calls
- Added SettlementPending, SettlementDeclined transaction statuses

## 2.33.0

- Add descriptor url support

## 2.32.0

- Allow credit card verification options to be passed outside of the nonce for PaymentMethod.create
- Allow billing_address parameters and billing_address_id to be passed outside of the nonce for PaymentMethod.create
- Add Subscriptions to paypal accounts
- Add PaymentMethod.update
- Add fail_on_duplicate_payment_method option to PaymentMethod.create
- Add support for dispute webhooks

## 2.31.0

- Add support for v.zero SDKs.

## 2.30.1

- Make webhook parsing more robust with newlines
- Add messages to InvalidSignature exceptions

## 2.30.0

- Include Dispute information on Transaction
- Search for Transactions disputed on a certain date

## 2.29.1

- Properly expose SsnLastFour for merchant accounts

## 2.29.0

- Disbursement Webhooks

## 2.28.1

- Allow a service fee of 0 in transaction create.
- Expose current billing cycle on add ons and discounts.
- Accept billing address id in transaction create.

## 2.28.0

- Merchant account find API

## 2.27.0

- Merchant account update API
- Merchant account create API v2

## 2.26.1

- Use new Braintree Gateway API endpoints

## 2.26.0

- Adds support for Partnerships

## 2.25.4

- Add unrecognized to enumerables, fraud to GatewayRejectionReason

## 2.25.3

- Fixed typo with DeviceData.

## 2.25.2

- Fixed typo in MerchantAccount.

## 2.25.0

- Adds HoldInEscrow method
- Add error codes for verification not supported error
- Add CompanyName and TaxId to merchant account create
- Adds CancelRelease method
- Adds ReleaseFromEscrow functionality
- Adds Phone to applicant details.
- Adds merchant account phone error code.

## 2.24.1

- Expose image_url attribute on credit cards

## 2.24.0

- Adds device data to transactions, customers, and credit cards.

## 2.23.1

- Set .NET Framework compatibility back to 2.0+

## 2.23.0

- Adds disbursement details to transactions.
- Adds image url to transactions.

## 2.22.0

- Adds Venmo Touch support.

## 2.21.0

- Fixes bug with zero dollar subscriptions.

## 2.20.0

- Adds channel field to transactions.

## 2.19.0

- Adds country of issuance and issuing bank

## 2.18.0

- Add verification search

## 2.17.0

- Additional card information, such as prepaid, debit, commercial, Durbin regulated, healthcare, and payroll, are returned on credit card responses
- Allows transactions to be specified as recurring

## 2.16.0

- Add prepaid attribute to credit card (possible values include Yes, No, Unknown)

## 2.15.1

- Fix warnings for Visual Studio

## 2.15.0

- Adds webhook gateways for parsing, verifying, and testing incoming notifications

## 2.14.0

- Adds search for duplicate credit cards given a payment method token
- Adds flag to fail saving credit card to vault if card is duplicate
- Changes `internal` functions to `protected internal` functions for easier unit testing

## 2.13.4

- Exposes plan_id on transactions

## 2.13.3

- Fixed Date parsing for hosts ahead of UTC (thanks to Jasmin Muharemovic)

## 2.13.2

- Added error code for invalid purchase order number

## 2.13.1

- Added error message for merchant accounts that do not support refunds

## 2.13.0

- Added ability to retrieve all Plans, Addons, and Discounts
- Added Transaction cloning

## 2.12.0

- Added SettlementBatchSummary

## 2.11.0

- Added Subscription to Transaction
- Added flag to store in vault only when a transaction is successful
- Added new error code

## 2.10.0

- Added a new transaction state, AUTHORIZATION_EXPIRED.
- Enabled searching by AuthorizationExpiredAt.

## 2.9.0

- Added NextBillingDate and TransactionId to subscription search
- Added AddressCountryName to customer search
- Added new error codes

## 2.8.0

- Added Customer search
- Added dynamic descriptors to Subscriptions and Transactions
- Added level 2 fields to Transactions:
  - TaxAmount
  - TaxExempt
  - PurchaseOrderNumber

## 2.7.2

- Added BillingAddressId to CreditCardRequest
- Allow searching on Subscriptions that are currently in a trial period using InTrialPeriod

## 2.7.1

- Added support for non-US cultures. Decimal values are now correctly formatted for the gateway and parsed for the client.

## 2.7.0

- Added ability to perform multiple partial refunds on Transactions
- Added RevertSubscriptionOnProrationFailure flag to Subscription update that specifies how a Subscription should react to a failed proration charge
- Deprecated Transaction RefundId in favor of RefundIds
- Deprecated Subscription NextBillAmount in favor of NextBillingPeriodAmount
- Added new properties to Subscription:
  - Balance
  - PaidThroughDate
  - NextBillingPeriodAmount

## 2.6.0

- Added AddOns/Discounts
- Enhanced Subscription search
- Enhanced Transaction search
- Made gateway operations threadsafe when using multiple configurations
- Added VerificationStatus Enumeration
- Added EXPIRED and PENDING statuses to Subscription
- Allowed ProrateCharges to be specified on Subscription update
- Added AddOn/Discount details to Transactions that were created from a Subscription
- All Braintree Exceptions now inherit from BraintreeException superclass
- Added new properties to Subscription:
  - BillingDayOfMonth
  - DaysPastDue
  - FirstBillingDate
  - NeverExpires
  - NumberOfBillingCycles

## 2.5.1

- Updated the Environment class to lazily use environment variables -- this enables use when access to environment variables is restricted

## 2.5.0

- Added ability to specify Country using CountryName, CountryCodeAlpha2, CountryCodeAlpha3, or CountryCodeNumeric
- Added GatewayRejectionReason to Transaction and Verification
- Added Message to Result
- Added BuildTrData method to TransparentRedirectGateway

## 2.4.0

- Added unified TransparentRedirect url and confirm methods and deprecated old methods
- Renamed CreditCard.Default to IsDefault
- Added methods to CreditCardGateway to allow searching on expiring and expired credit cards
- Added ability to update a customer, credit card, and billing address in one request
- Allow updating the payment method token on a subscription
- Added methods to navigate between a Transaction and its refund (in both directions)

## 2.3.0

- Return AvsErrorResponseCode, AvsPostalCodeResponseCode, AvsStreetAddressResponseCode, CurrencyIsoCode, CvvResponseCode with Transaction
- Return CreatedAt, UpdatedAt with Address
- Allow verification against a specified merchant account when creating or updating a CreditCard
- Return SubscriptionId with Transaction

## 2.2.0

- Prevent race condition when pulling back collection results -- search results represent the state of the data at the time the query was run
- Rename ResourceCollection's ApproximateCount to MaximumCount because items that no longer match the query will not be returned in the result set
- Correctly handle HTTP error 426 (Upgrade Required) -- the error code is returned when your client library version is no longer compatible with the gateway
- Properly handle Transaction Options in TR Data

## 2.1.0

- Added transaction advanced search
- Added ability to partially refund transactions
- Added ability to manually retry past-due subscriptions
- Added new transaction error codes
- Allow merchant account to be specified when creating transactions
- Allow creating a transaction with a vault customer and new credit card
- Allow existing billing address to be updated when updating credit card
- **Backwards incompatible change**: CreditCardRequest.BillingAddress has changed from type AddressRequest to CreditCardAddressRequest

## 2.0.0

- Updated IsSuccess() on transaction results to return false on declined transactions
- Search results now implement IEnumerable and will automatically paginate data

## 1.2.1

- Escape all XML
- Updated quick start example in README

## 1.2.0

- Added subscription search
- Return associated subscriptions when finding credit cards
- Added option to change default credit card for a customer
- Added an All method to ValidationErrors to return List of all errors at that level
- Added a DeepAll method to ValidationErrors to return List of all errors at that level and all errors below
- Renamed DeepSize() to DeepCount
- Added ProcessorAuthorizationCode to Transaction
- Allow setting MerchantAccountId when creating or updating subscriptions
- Updated ForObject to return an empty ValidationErrors object instead of null if there are no errors
- Raise down for maintenance exception instead of forged query string when down for maintenance
- Fixed bug in TotalPages if there are zero total items
