#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class ConfigurationException : BraintreeException
    {
        public ConfigurationException(string message) : base(message) {}
    }
}
