namespace Braintree
{
    public class AddPlanModificationRequest : PlanModificationRequest
    {
        public string InheritedFromId { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).AddElement("inherited-from-id", InheritedFromId);
        }
    }
}