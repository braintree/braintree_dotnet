#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class AddOn : Modification
    {
        protected internal AddOn(NodeWrapper node) : base(node) {
        }
    }
}
