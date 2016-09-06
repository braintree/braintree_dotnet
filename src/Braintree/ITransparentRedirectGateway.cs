#pragma warning disable 1591

namespace Braintree
{
    public interface ITransparentRedirectGateway
    {
        string Url { get; }

        string BuildTrData(Request request, string redirectURL);
        Result<CreditCard> ConfirmCreditCard(string queryString);
        Result<Customer> ConfirmCustomer(string queryString);
        Result<Transaction> ConfirmTransaction(string queryString);
    }
}
