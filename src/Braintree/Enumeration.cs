#pragma warning disable 1591

using System;

namespace Braintree
{
    public abstract class Enumeration
    {
        protected string Name;

        protected Enumeration(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
