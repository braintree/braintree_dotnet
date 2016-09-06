namespace Braintree
{
    public class OAuthConnectUrlUserRequest : Request
    {
        public string Country { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DobYear { get; set; }
        public string DobMonth { get; set; }
        public string DobDay { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }

        public override string ToQueryString(string root)
        {
            var builder = new RequestBuilder("user");
            builder.AddElement("country", Country);
            builder.AddElement("email", Email);
            builder.AddElement("first_name", FirstName);
            builder.AddElement("last_name", LastName);
            builder.AddElement("phone", Phone);
            builder.AddElement("dob_year", DobYear);
            builder.AddElement("dob_month", DobMonth);
            builder.AddElement("dob_day", DobDay);
            builder.AddElement("street_address", StreetAddress);
            builder.AddElement("locality", Locality);
            builder.AddElement("region", Region);
            builder.AddElement("postal_code", PostalCode);
            return builder.ToQueryString();
        }
    }
}
