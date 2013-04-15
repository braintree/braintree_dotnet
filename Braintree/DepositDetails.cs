#pragma warning disable 1591

using System;

namespace Braintree
{
    public class DepositDetails
    {
        public Decimal? SettlementAmount { get; protected set; }
        public String SettlementCurrencyIsoCode { get; protected set; }
        public String SettlementCurrencyExchangeRate { get; protected set; }
        public Boolean? FundsHeld { get; protected set; }
        public DateTime? DepositDate { get; protected set; }
        public DateTime? DisbursedAt { get; protected set; }

        protected internal DepositDetails(NodeWrapper node)
        {
            SettlementAmount = node.GetDecimal("settlement-amount");
            SettlementCurrencyIsoCode = node.GetString("settlement-currency-iso-code");
            SettlementCurrencyExchangeRate = node.GetString("settlement-currency-exchange-rate");
            FundsHeld = node.GetBoolean("funds-held");
            DepositDate = node.GetDateTime("deposit-date");
            DisbursedAt = node.GetDateTime("disbursed-at");
        }

        public Boolean IsValid()
        {
            return DepositDate != null;
        }
    }
}
