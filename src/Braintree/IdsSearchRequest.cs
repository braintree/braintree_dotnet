#pragma warning disable 1591

namespace Braintree
{
    public class IdsSearchRequest : SearchRequest
    {
        public MultipleValueNode<IdsSearchRequest, string> Ids => new MultipleValueNode<IdsSearchRequest, string>("ids", this);
    }
}
