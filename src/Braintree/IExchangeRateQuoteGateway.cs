namespace Braintree
{
    public interface IExchangeRateQuoteGateway
    {
        ExchangeRateQuotePayload Generate(ExchangeRateQuoteRequest request);
    }
}