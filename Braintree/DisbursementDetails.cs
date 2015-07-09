#pragma warning disable 1591

using System;

namespace Braintree
{
    public class DisbursementDetails
    {
        public decimal? SettlementAmount { get; protected set; }
        public string SettlementCurrencyIsoCode { get; protected set; }
        public string SettlementCurrencyExchangeRate { get; protected set; }
        public bool? FundsHeld { get; protected set; }
        public bool? Success { get; protected set; }
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

        public bool IsValid()
        {
            return DisbursementDate != null;
        }
    }
}
