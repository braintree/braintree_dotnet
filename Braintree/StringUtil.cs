#pragma warning disable 1591

using System;
using System.Text.RegularExpressions;

namespace Braintree
{
    public class StringUtil
    {
        public static String Dasherize(String str)
        {
            if (str == null) return null;

            str = Regex.Replace(str, @"([A-Z]+)([A-Z][a-z])", "$1-$2");
            return Regex.
                Replace(str, @"([a-z])([A-Z])", "$1-$2").
                Replace("_", "-").
                ToLower();
        }

        public static String Underscore(String str)
        {
            if (str == null) return null;

            str = Regex.Replace(str, @"([A-Z]+)([A-Z][a-z])", "$1_$2");
            return Regex.
                Replace(str, @"([a-z])([A-Z])", "$1_$2").
                Replace("-", "_").
                ToLower();
        }
    }
}
