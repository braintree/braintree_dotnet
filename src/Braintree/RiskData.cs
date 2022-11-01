using System;
using System.Collections.Generic;

namespace Braintree
{
    public class RiskData
    {
        // NEXT_MAJOR_VERSION these should all be Pascal case,
        // but we can't add those now without setting off a
        // CLS-compliance error (CS3005)
        public virtual bool? deviceDataCaptured { get; protected set; }
        public virtual LiabilityShift LiabilityShift { get; protected set; }
        public virtual List<string> DecisionReasons { get; protected set; }
        public virtual string decision { get; protected set; }
        public virtual string fraudServiceProvider { get; protected set; }
        public virtual string id { get; protected set; }
        public virtual string TransactionRiskScore { get; protected set; }

        public RiskData(NodeWrapper node)
        {
            if (node == null)
                return;

            decision = node.GetString("decision");
            DecisionReasons = new List<string>();
            deviceDataCaptured = node.GetBoolean("device-data-captured");
            fraudServiceProvider = node.GetString("fraud-service-provider");
            id = node.GetString("id");
            TransactionRiskScore = node.GetString("transaction-risk-score");
            foreach (var stringNode in node.GetList("decision-reasons/item")) 
            {
                DecisionReasons.Add(stringNode.GetString("."));
            }
            var liabilityShiftXmlNode = node.GetNode("liability-shift");

            if (liabilityShiftXmlNode != null)
            {
                LiabilityShift = new LiabilityShift(liabilityShiftXmlNode);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal RiskData() { }
    }
}

