using System.Threading.Tasks;

namespace Braintree
{
    public interface IPayPalPaymentResourceGateway
    {
        Result<PaymentMethodNonce>  Update(PayPalPaymentResourceRequest request);
        Task<Result<PaymentMethodNonce>> UpdateAsync(PayPalPaymentResourceRequest request);
    }
}