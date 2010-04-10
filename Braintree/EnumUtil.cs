#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class EnumUtil
    {
        public static System.Enum Find(System.Type enumType, String name, String defaultValue) 
        {
            if (Enum.IsDefined(enumType, name.ToUpper()))
            {
                return (System.Enum)Enum.Parse(enumType, name, true);
            }
            else
            {
                return (System.Enum)Enum.Parse(enumType, defaultValue, true);
            }
        }
    }
}
