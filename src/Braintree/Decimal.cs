#pragma warning disable 1591

using System;

namespace Braintree
{
    public class Decimal
    {
        private string value;

        public Decimal(string value)
        {
            this.value = value;
        }

        static public implicit operator Decimal(decimal value)
        {
            return new Decimal(value.ToString());
        }

        static public implicit operator Decimal(string value)
        {
            return new Decimal(value);
        }

        public override string ToString()
        {
            return value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return Equals((Decimal) other);
        }

        private bool Equals(Decimal other)
        {
            return string.Equals(this.value, other.value);
        }
    }
}
