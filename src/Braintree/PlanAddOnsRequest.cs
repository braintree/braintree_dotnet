namespace Braintree
{
    public class PlanAddOnsRequest : Request
    {
        public AddPlanModificationRequest[] Add { get; set; }
        public UpdatePlanModificationRequest[] Update { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("add", Add).
                AddElement("update", Update);
        }
    }
}