#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    [Serializable]
    public class ValidationError
    {
        public String Attribute { get; protected set; }
        public ValidationErrorCode Code { get; protected set; }
        public String Message { get; protected set; }

        public ValidationError(String attribute, String code, String message)
        {
            Attribute = attribute;
            Code = (ValidationErrorCode)Int32.Parse(code);
            Message = message;
        }
    }
}
