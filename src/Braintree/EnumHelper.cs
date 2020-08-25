#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{
    public static class EnumHelper
    {
        public static T FindEnum<T>(string description, T defaultValue) where T : struct, Enum
        {
            if (description == null) return defaultValue;
            Array values = Enum.GetValues(typeof(T));

            foreach (T value in values)
            {
                if (string.Equals(description, value.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        // Some enums properties can be null, so we need a separate function for those scenarios
        public static T? FindEnum<T>(string description, T? defaultValue = null) where T : struct, Enum
        {
            if (description == null) return defaultValue;
            Array values = Enum.GetValues(typeof(T));

            foreach (T value in values)
            {
                if (string.Equals(description, value.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        public static string GetDescription(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return null;
            }

            DescriptionAttribute[] attributes = (DescriptionAttribute[])enumValue
                .GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : null;
        }
    }
}
