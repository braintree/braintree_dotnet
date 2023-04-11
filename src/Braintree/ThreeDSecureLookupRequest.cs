using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace Braintree
{
    public class ThreeDSecureLookupRequest : Request
    {
        public ThreeDSecureLookupAdditionalInformation AdditionalInformation { get; set; }
        public string Amount { get; set; }
        public string AuthorizationFingerprint { get; set; }
        public ThreeDSecureLookupAddress BillingAddress { get; set; }
        public string BraintreeLibraryVersion { get; set; }
        public string BrowserAcceptHeader { get; set; }
        public string BrowserColorDepth { get; set; }
        public bool? BrowserJavaEnabled { get; set; }
        public bool? BrowserJavascriptEnabled { get; set; }
        public string BrowserLanguage { get; set; }
        public string BrowserScreenHeight { get; set; }
        public string BrowserScreenWidth { get; set; }
        public string BrowserTimeZone { get; set; }
        public bool? ChallengeRequested { get; set; }
        public bool? DataOnlyRequested { get; set; }
        public string DeviceChannel { get; set; }
        public string DfReferenceId { get; set; }
        public string Email { get; set; }
        public bool? ExemptionRequested { get; set; }
        public string IpAddress { get; set; }
        public string Nonce { get; set; }
        public string RequestedExemptionType { get; set; }
        public string UserAgent { get; set; }


        private dynamic _ClientMetadata;
        public virtual dynamic ClientData
        {
            get => _ClientMetadata;
            set
            {
                var metadata = JsonConvert.DeserializeObject<dynamic>(value);

                Nonce = metadata.nonce;
                AuthorizationFingerprint = metadata.authorizationFingerprint;
                BraintreeLibraryVersion = metadata.braintreeLibraryVersion;
                DfReferenceId = metadata.dfReferenceId;
                _ClientMetadata = metadata.clientMetadata;
            }
        }

        public string ToJSON() {
            Dictionary<string, object> additionalInfo;
            var json = new Dictionary<string, object>();
            var meta = new Dictionary<string, object>();

            if (AdditionalInformation != null) {
                additionalInfo = AdditionalInformation.ToDictionary();
            } else {
                additionalInfo = new Dictionary<string, object>();
            }

            meta.Add("platform", "dotnet");
#if netcore
            meta.Add("sdkVersion", typeof(ThreeDSecureLookupRequest).GetTypeInfo().Assembly.GetName().Version.ToString());
#else
            meta.Add("sdkVersion", typeof(ThreeDSecureLookupRequest).Assembly.GetName().Version.ToString());
#endif
            meta.Add("source", "http");

            json.Add("_meta", meta);

            json.Add("authorizationFingerprint", AuthorizationFingerprint);
            json.Add("amount", Amount);
            json.Add("braintreeLibraryVersion", BraintreeLibraryVersion);
            json.Add("browserHeader", BrowserAcceptHeader);
            json.Add("browserColorDepth", BrowserColorDepth);
            json.Add("browserLanguage", BrowserLanguage);
            json.Add("browserScreenHeight", BrowserScreenHeight);
            json.Add("browserScreenWidth", BrowserScreenWidth);
            json.Add("browserTimeZone", BrowserTimeZone);
            json.Add("clientMetadata", ClientData);
            json.Add("df_reference_id", DfReferenceId);
            json.Add("deviceChannel", DeviceChannel);
            json.Add("email", Email);
            json.Add("ipAddress", IpAddress);
            json.Add("userAgent", UserAgent);

            if (BrowserJavaEnabled != null) {
                json.Add("browserJavaEnabled", BrowserJavaEnabled);
            }
            if (BrowserJavascriptEnabled != null) {
                json.Add("browserJavascriptEnabled", BrowserJavascriptEnabled);
            }
            if (ChallengeRequested != null) {
                json.Add("challengeRequested", ChallengeRequested);
            } 
            if (DataOnlyRequested != null) {
                json.Add("dataOnlyRequested", DataOnlyRequested);
            }
            if (ExemptionRequested != null) {
                json.Add("exemptionRequested", ExemptionRequested);
            }
            if (RequestedExemptionType != null) {
                json.Add("requestedExemptionType", RequestedExemptionType);
            }

            if (BillingAddress != null) {
                additionalInfo.Add("billingGivenName", BillingAddress.GivenName);
                additionalInfo.Add("billingSurname", BillingAddress.Surname);
                additionalInfo.Add("billingPhoneNumber", BillingAddress.PhoneNumber);
                additionalInfo.Add("billingCity", BillingAddress.Locality);
                additionalInfo.Add("billingCountryCode", BillingAddress.CountryCodeAlpha2);
                additionalInfo.Add("billingLine1", BillingAddress.StreetAddress);
                additionalInfo.Add("billingLine2", BillingAddress.ExtendedAddress);
                additionalInfo.Add("billingPostalCode", BillingAddress.PostalCode);
                additionalInfo.Add("billingState", BillingAddress.Region);
            }

            json.Add("additional_info", additionalInfo);

            return JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None);
        }
    }
}
