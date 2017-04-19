using System;

namespace Braintree
{
    public class IbanBankAccount
    {
        public virtual string AccountHolderName { get; protected set; }
        public virtual string IbanAccountNumberLast4 { get; protected set; }
        public virtual string IbanCountry { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string MaskedIban { get; protected set; }
        public virtual string Bic { get; protected set; }

        protected internal IbanBankAccount(NodeWrapper node)
        {
            AccountHolderName = node.GetString("account-holder-name");
            IbanAccountNumberLast4 = node.GetString("iban-account-number-last-4");
            IbanCountry = node.GetString("iban-country");
            Description = node.GetString("description");
            MaskedIban = node.GetString("masked-iban");
            Bic = node.GetString("bic");
        }

        [Obsolete("Mock Use Only")]
        protected internal IbanBankAccount() { }
    }
}
