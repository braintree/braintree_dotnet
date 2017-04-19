#pragma warning disable 1591

namespace Braintree
{
    public interface IIdealPaymentGateway
    {
        IdealPayment Find(string idealPaymentId);
        Result<Transaction> Sale(string idealPaymentId, TransactionRequest transactionRequest);
    }
}
