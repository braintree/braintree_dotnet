#pragma warning disable 1591


namespace Braintree
{
    public class ValidationError
    {
        public string Attribute { get; protected set; }
        public ValidationErrorCode Code { get; protected set; }
        public string Message { get; protected set; }

        public ValidationError(string attribute, string code, string message)
        {
            Attribute = attribute;
            Code = (ValidationErrorCode)int.Parse(code);
            Message = message;
        }
    }
}
