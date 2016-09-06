#pragma warning disable 1591

namespace Braintree
{
    public interface IWebhookNotificationGateway
    {
        WebhookNotification Parse(string signature, string payload);
        string Verify(string challenge);
    }
}
