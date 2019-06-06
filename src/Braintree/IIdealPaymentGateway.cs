#pragma warning disable 1591

// NEXT_MAJOR_VERSION Remove this class as legacy Ideal has been removed/disabled in the Braintree Gateway
// DEPRECATED If you're looking to accept iDEAL as a payment method contact accounts@braintreepayments.com for a solution.
namespace Braintree
{
    public interface IIdealPaymentGateway
    {
        IdealPayment Find(string idealPaymentId);
        Result<Transaction> Sale(string idealPaymentId, TransactionRequest transactionRequest);
    }
}
