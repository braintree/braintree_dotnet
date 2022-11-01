namespace Braintree
{
    public class ExchangeRateQuoteGateway : IExchangeRateQuoteGateway
    {
        private readonly BraintreeGateway gateway;
        const string EXCHANGE_RATE_QUOTE_MUTATION = @"
            mutation ($exchangeRateQuoteRequest: GenerateExchangeRateQuoteInput!) {
                generateExchangeRateQuote(input: $exchangeRateQuoteRequest) {
                quotes {
                    id
                    baseAmount {value, currencyCode}
                    quoteAmount {value, currencyCode}
                    exchangeRate
                    tradeRate
                    expiresAt
                    refreshesAt
                }
                }
            }";

        protected internal ExchangeRateQuoteGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
        }

        public virtual ExchangeRateQuotePayload Generate(ExchangeRateQuoteRequest request)
        {
            var graphQLCleint = gateway.GraphQLClient;
            var variables = request.ToGraphQLVariables();
            var response = graphQLCleint.Query(EXCHANGE_RATE_QUOTE_MUTATION, variables);
            return new ExchangeRateQuotePayload(response);
        }
    }
}