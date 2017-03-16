#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    public interface IPaymentMethodGateway
    {
        Result<PaymentMethod> Create(PaymentMethodRequest request);
        Task<Result<PaymentMethod>> CreateAsync(PaymentMethodRequest request);
        Result<PaymentMethod> Delete(string token);
        Task<Result<PaymentMethod>> DeleteAsync(string token);
        Result<PaymentMethod> Delete(string token, PaymentMethodDeleteRequest request);
        PaymentMethod Find(string token);
        Task<PaymentMethod> FindAsync(string token);
        Result<PaymentMethod> Update(string token, PaymentMethodRequest request);
        Task<Result<PaymentMethod>> UpdateAsync(string token, PaymentMethodRequest request);
        Result<PaymentMethodNonce> Grant(string token, PaymentMethodGrantRequest request);
        Result<PaymentMethod> Revoke(string token);
    }
}
