#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    public class WebhookGateway
    {
        private BraintreeService Service;

        protected internal WebhookGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual WebhookNotification Parse(string signature, string payload)
        {
            ValidateSignature(signature, payload);
            string xmlPayload = Encoding.Default.GetString(Convert.FromBase64String(payload));
            NodeWrapper node = new NodeWrapper(Service.StringToXmlNode(xmlPayload));
            return new WebhookNotification(node, Service);
        }

        public virtual string Verify(string challenge)
        {
            string digest = new Crypto().HmacHash(Service.PrivateKey, challenge);
            return String.Format("{0}|{1}", Service.PublicKey, digest.ToLower());
        }

        private void ValidateSignature(string signature, string payload)
        {
            string matchingSignature = null;
            string[] signaturePairs = signature.Split('&');

            foreach (string signaturePair in signaturePairs)
            {
                if (signaturePair.IndexOf('|') >= 0)
                {
                    String[] candidatePair = signaturePair.Split('|');
                    if (Service.PublicKey.Equals(candidatePair[0]))
                    {
                        matchingSignature = candidatePair[1];
                        break;
                    }
                }
            }

            string computedSignature = new Crypto().HmacHash(Service.PrivateKey, payload).ToLower();
            if (!computedSignature.Equals(matchingSignature))
            {
                throw new InvalidSignatureException();
            }
        }
    }
}