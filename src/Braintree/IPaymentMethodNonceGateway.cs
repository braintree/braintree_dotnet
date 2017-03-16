#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    public interface IPaymentMethodNonceGateway
    {
        Result<PaymentMethodNonce> Create(string token);
        Task<Result<PaymentMethodNonce>> CreateAsync(string token);
        PaymentMethodNonce Find(string nonce);
        Task<PaymentMethodNonce> FindAsync(string nonce);
    }
}
