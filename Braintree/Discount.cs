#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class Discount : Modification
    {
        protected internal Discount(NodeWrapper node) : base(node) {
        }
    }
}
