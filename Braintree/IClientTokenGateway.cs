namespace Braintree
{
    public interface IClientTokenGateway
    {
        string Generate(ClientTokenRequest request = null);
    }
}