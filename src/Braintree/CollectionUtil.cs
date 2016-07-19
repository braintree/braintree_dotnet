#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CollectionUtil
    {
        public static object Find(object[] items, string name, object defaultValue)
        {
            foreach (var item in items) {
                if (name != null && item.ToString().ToUpper().Equals(name.ToUpper())) {
                    return item;
                }
            }
            return defaultValue;
        }
    }
}
