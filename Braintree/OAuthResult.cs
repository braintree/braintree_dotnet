using System;

namespace Braintree
{
    public class OAuthResult
    {
        public OAuthResult(NodeWrapper node)
        {
            if (node == null) return;

            Result = node.GetBoolean("success");
        }

        public bool? Result { get; set; }
    }
}
