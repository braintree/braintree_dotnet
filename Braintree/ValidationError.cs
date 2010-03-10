#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class ValidationError
    {
        public ValidationErrorCode Code { get; protected set; }
        public String Message { get; protected set; }

        public ValidationError(String code, String message)
        {
            Code = (ValidationErrorCode)Int32.Parse(code);
            Message = message;
        }
    }
}
