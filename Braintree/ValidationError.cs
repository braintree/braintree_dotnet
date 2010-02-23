using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class ValidationError
    {
        public String Code { get; protected set; }
        public String Message { get; protected set; }

        public ValidationError(String code, String message)
        {
            Code = code;
            Message = message;
        }
    }
}
