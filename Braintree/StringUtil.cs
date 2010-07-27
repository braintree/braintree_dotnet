using System;
using System.Text.RegularExpressions;

namespace Braintree
{
    public class StringUtil
    {
        public static String Dasherize(String str)
        {
            return str == null ? null : Regex.Replace(str, @"([a-z])([A-Z])", "$1-$2").Replace("_", "-").ToLower();
        }

        public static String Underscore(String str)
        {
            return str == null ? null : Regex.Replace(str, @"([a-z])([A-Z])", "$1_$2").Replace("-", "_").ToLower();
        }

    }
}

