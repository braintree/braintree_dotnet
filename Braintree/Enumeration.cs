#pragma warning disable 1591

using System;

namespace Braintree
{
    public abstract class Enumeration
    {
        protected String Name;

        protected Enumeration(String name)
        {
            Name = name;
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
