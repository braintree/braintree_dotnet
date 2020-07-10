using System;

namespace Braintree
{
    public class AuthenticationInsightResponse
    {
        public virtual string ScaIndicator { get; protected set; }
        public virtual string RegulationEnvironment { get; protected set; }

        public AuthenticationInsightResponse(NodeWrapper node)
        {
            if (node == null)
                return;

            ScaIndicator = node.GetString("sca-indicator");
            RegulationEnvironment = node.GetString("regulation-environment");
        }
    }
}

