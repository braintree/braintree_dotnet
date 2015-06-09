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
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal WebhookNotificationGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual WebhookNotification Parse(string signature, string payload)
        {
            ValidateSignature(signature, payload);
            string xmlPayload = Encoding.Default.GetString(Convert.FromBase64String(payload));
            NodeWrapper node = new NodeWrapper(Service.StringToXmlNode(xmlPayload));
            return new WebhookNotification(node, Gateway);
        }

        public virtual string Verify(string challenge)
        {
            Match match = Regex.Match (challenge, @"^[a-f0-9]{20,32}$");
            if (!match.Success)
            {
                throw new InvalidChallengeException ("challenge contains non-hex characters");
            }
            string digest = new Sha1Hasher().HmacHash(Service.PrivateKey, challenge);
            return String.Format("{0}|{1}", Service.PublicKey, digest.ToLower());
        }

        private bool PayloadMatches(string signature, string payload)
        {
            Sha1Hasher sha1Hasher = new Sha1Hasher();
            string computedSignature = sha1Hasher.HmacHash(Service.PrivateKey, payload).ToLower();
            Crypto crypto = new Crypto();
            return crypto.SecureCompare (computedSignature, signature);
        }

        private void ValidateSignature(string signature, string payload)
        {
            Match match = Regex.Match (payload, @"[^A-Za-z0-9+=/\n]");
            if (match.Success)
            {
                throw new InvalidSignatureException ("payload contains illegal characters");
            }

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
