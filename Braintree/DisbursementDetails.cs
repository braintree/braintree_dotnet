#pragma warning disable 1591

using System;

namespace Braintree
{
    public class DisbursementDetails
    {
        public Decimal? SettlementAmount { get; protected set; }
        public String SettlementCurrencyIsoCode { get; protected set; }
        public String SettlementCurrencyExchangeRate { get; protected set; }
        public Boolean? FundsHeld { get; protected set; }
        public Boolean? Success { get; protected set; }
        public DateTime? DisbursementDate { get; protected set; }

        protected internal DisbursementDetails(NodeWrapper node)
        {
            SettlementAmount = node.GetDecimal("settlement-amount");
            SettlementCurrencyIsoCode = node.GetString("settlement-currency-iso-code");
            SettlementCurrencyExchangeRate = node.GetString("settlement-currency-exchange-rate");
            FundsHeld = node.GetBoolean("funds-held");
            Success = node.GetBoolean("success");
            DisbursementDate = node.GetDateTime("disbursement-date");
        }

        public Boolean IsValid()
        {
            return DisbursementDate != null;
        }
    }
}
