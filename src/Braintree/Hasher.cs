#pragma warning disable 1591

namespace Braintree
{
    public interface Hasher
    {
        string HmacHash(string key, string content);
    }
}
