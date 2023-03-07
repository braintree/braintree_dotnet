namespace Braintree
{
    public class UpdatePlanModificationRequest : PlanModificationRequest
    {
        public string ExistingId { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).
                AddElement("existing-id", ExistingId);
        }
    }
}