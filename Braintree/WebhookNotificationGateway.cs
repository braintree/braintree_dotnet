#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Braintree.Exceptions;
using System.Text.RegularExpressions;

namespace Braintree
{
    public class WebhookNotificationGateway
    {
        private BraintreeService service;
        private BraintreeGateway gateway;

        protected internal WebhookNotificationGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            this.service = new BraintreeService(gateway.Configuration);
        }

        public virtual WebhookNotification Parse(string signature, string payload)
        {
            ValidateSignature(signature, payload);
            var xmlPayload = Encoding.Default.GetString(Convert.FromBase64String(payload));
            var node = new NodeWrapper(service.StringToXmlNode(xmlPayload));
            return new WebhookNotification(node, gateway);
        }

        public virtual string Verify(string challenge)
        {
            var match = Regex.Match (challenge, @"^[a-f0-9]{20,32}$");
            if (!match.Success)
            {
                throw new InvalidChallengeException ("challenge contains non-hex characters");
            }
            string digest = new Sha1Hasher().HmacHash(service.PrivateKey, challenge);
            return string.Format("{0}|{1}", service.PublicKey, digest.ToLower());
        }

        private bool PayloadMatches(string signature, string payload)
        {
            var sha1Hasher = new Sha1Hasher();
            string computedSignature = sha1Hasher.HmacHash(service.PrivateKey, payload).ToLower();
            var crypto = new Crypto();
            return crypto.SecureCompare (computedSignature, signature);
        }

        private void ValidateSignature(string signature, string payload)
        {
            var match = Regex.Match (payload, @"[^A-Za-z0-9+=/\n]");
            if (match.Success)
            {
                throw new InvalidSignatureException ("payload contains illegal characters");
            }

            string matchingSignature = null;
            string[] signaturePairs = signature.Split('&');

            foreach (var signaturePair in signaturePairs)
            {
                if (signaturePair.IndexOf('|') >= 0)
                {
                    string[] candidatePair = signaturePair.Split('|');
                    if (service.PublicKey.Equals(candidatePair[0]))
                    {
                        matchingSignature = candidatePair[1];
                        break;
                    }
                }
            }

            if (matchingSignature == null)
            {
                throw new InvalidSignatureException ("no matching public key");
            }

            if (!(PayloadMatches(matchingSignature, payload) || PayloadMatches(matchingSignature, payload + "\n")))
            {
                throw new InvalidSignatureException ("signature does not match payload - one has been modified");
            }
        }
    }
}
