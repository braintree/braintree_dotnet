#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public interface IWebhookTestingGateway
    {
        Dictionary<string, string> SampleNotification(WebhookKind kind, string id);
    }
}
