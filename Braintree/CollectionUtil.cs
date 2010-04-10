#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CollectionUtil
    {
        public static object Find(object[] items, String name, object defaultValue)
        {
            foreach (object item in items) {
                if (item.ToString().ToUpper().Equals(name.ToUpper())) {
                    return item;
                }
            }
            return defaultValue;
        }
    }
}
