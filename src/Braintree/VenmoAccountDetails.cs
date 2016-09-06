namespace Braintree
{
    public class VenmoAccountDetails
    {
        public string Token { get; protected set; }
        public string Username { get; protected set; }
        public string VenmoUserId { get; protected set; }
        public string ImageUrl { get; protected set; }
        public string SourceDescription { get; protected set; }

        protected internal VenmoAccountDetails(NodeWrapper node)
        {
            Token = node.GetString("token");
            Username = node.GetString("username");
            VenmoUserId = node.GetString("venmo-user-id");
            ImageUrl = node.GetString("image-url");
            SourceDescription = node.GetString("source-description");
        }
    }
}
