namespace Braintree
{
    public class PlanModificationRequest : Request
    {
        public decimal Amount { get; set; }
        public string Description { get; set;}
        public string Name { get; set;}
        public int NumberOfBillingCycles { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("description", Description).
                AddElement("name", Name).
                AddElement("number-of-billing-cycles", NumberOfBillingCycles);
        }
    }
}