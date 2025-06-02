using Braintree.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Represents the customer recommendations information associated with a PayPal customer session.
    /// </summary>
    public class CustomerRecommendationsPayload
    {
        public virtual bool IsInPayPalNetwork { get; protected set; }
        public virtual CustomerRecommendations Recommendations { get; protected set; }

        public CustomerRecommendationsPayload(Dictionary<string, object> data)
        {
            if (
                data.ContainsKey("generateCustomerRecommendations")
                && data["generateCustomerRecommendations"] is Dictionary<string, object> customerRecommendations
            )
            {
                object objValue;
                bool boolValue = false;
                if (
                    customerRecommendations.TryGetValue("isInPayPalNetwork", out objValue)
                    && objValue != null
                    && bool.TryParse(objValue.ToString(), out boolValue)
                )
                {
                    IsInPayPalNetwork = boolValue;
                } else
                {
                    throw new ServerException();
                }

                var paymentRecommendations = ExtractPaymentRecommendations(customerRecommendations);
                Recommendations = new CustomerRecommendations(
                    paymentRecommendations
                );
            } else {
                throw new ServerException();
            }
        }

        private List<PaymentRecommendation> ExtractPaymentRecommendations(
            Dictionary<string, object> data
        )
        {
            var paymentRecommendations = new List<PaymentRecommendation>();
            if (data.ContainsKey("paymentRecommendations"))
            {
                var recommendationObjs = new List<Dictionary<string, object>>();
                var recommendationsList = ((JArray)data["paymentRecommendations"]).ToList();
                foreach (var recommendation in recommendationsList)
                {
                    recommendationObjs.Add(recommendation.ToObject<Dictionary<string, object>>());
                }
                foreach (var recommendationObj in recommendationObjs)
                {
                    if (
                        recommendationObj.ContainsKey("recommendedPriority")
                        && recommendationObj["recommendedPriority"] != null
                        && recommendationObj.ContainsKey("paymentOption")
                        && recommendationObj["paymentOption"] != null
                    )
                    {
                        var paymentOptionString = recommendationObj["paymentOption"].ToString();
                        var priorityValue = Convert.ToInt32(recommendationObj["recommendedPriority"]);
                        if (
                            Enum.TryParse<RecommendedPaymentOption>(
                                paymentOptionString,
                                false,
                                out var paymentOption
                            )
                        )
                        {
                            paymentRecommendations.Add(
                                new PaymentRecommendation(paymentOption, priorityValue)
                            );
                        }
                    }
                }

            }
            return paymentRecommendations;
            
        }
    }
}
