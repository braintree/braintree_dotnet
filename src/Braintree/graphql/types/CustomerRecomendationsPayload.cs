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
    /// <summary>
    /// Represents the customer recommendations information associated with a PayPal customer session.
    /// </summary>
    public class CustomerRecommendationsPayload
    {
        public virtual bool IsInPayPalNetwork { get; protected set; }
        public virtual CustomerRecommendations Recommendations { get; protected set; }

        public CustomerRecommendationsPayload(bool isPayPalNetwork, CustomerRecommendations customerRecommendations)
        {
            IsInPayPalNetwork = isPayPalNetwork;
            Recommendations = customerRecommendations;
        }

        public CustomerRecommendationsPayload(Dictionary<string, object> data)
        {

            if (
                data.ContainsKey("customerRecommendations")
                && data["customerRecommendations"] is Dictionary<string, object> customerRecommendations
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
                }
                CustomerRecommendations recommendations = ExtractCustomerRecommendations(customerRecommendations);
                if (recommendations != null)
                {
                    Recommendations = recommendations;
                }
            }
            else
            {
                throw new UnexpectedException();
            }
        }

        private static CustomerRecommendations ExtractCustomerRecommendations(
            Dictionary<string, object> customerRecommendations
        )
        {
            var paymentOptions = new List<PaymentOptions>();

            if (
                customerRecommendations.ContainsKey("recommendations") 
                && customerRecommendations["recommendations"] != null
                && customerRecommendations["recommendations"] is Dictionary<string, object> recommendationsData
            )
            {
                var recommendationObjs = new List<Dictionary<string, object>>();
                if (recommendationsData.ContainsKey("paymentOptions"))
                {
                    var recommendationsList = ((JArray)recommendationsData["paymentOptions"]).ToList();
                    foreach (var recommendation in recommendationsList)
                    {
                        recommendationObjs.Add(recommendation.ToObject<Dictionary<string, object>>());
                    }
                }

                foreach (var recommendationObj in recommendationObjs)
                {
                    if (
                        recommendationObj.ContainsKey("recommendedPriority")
                        && recommendationObj["paymentOption"] != null
                        && recommendationObj.ContainsKey("paymentOption")
                        && recommendationObj["recommendedPriority"] != null
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
                            paymentOptions.Add(
                                new PaymentOptions(priorityValue, paymentOption)
                            );
                        }
                    }
                }
            }

            return new CustomerRecommendations(paymentOptions);
        }
    }
}
