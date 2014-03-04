#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class ConfigurationException : BraintreeException
    {
        public ConfigurationException(String message) : base(message) {}
    }
}
