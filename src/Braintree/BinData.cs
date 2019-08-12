using System;

namespace Braintree
{
    public class BinData
    {
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }

        private string _CountryOfIssuance;

        public virtual string CountryOfIssuance
        {
            get
            {
                if (_CountryOfIssuance == "")
                {
                    return CreditCard.CountryOfIssuanceUnknown;
                }
                else
                {
                    return _CountryOfIssuance;
                }
            }
        }

        private string _IssuingBank;

        public virtual string IssuingBank
        {
            get
            {
                if (_IssuingBank == "")
                {
                    return CreditCard.IssuingBankUnknown;
                }
                else
                {
                    return _IssuingBank;
                }
            }
        }

        private string _ProductId;

        public virtual string ProductId
        {
            get
            {
                if (_ProductId == "")
                {
                    return CreditCard.ProductIdUnknown;
                }
                else
                {
                    return _ProductId;
                }
            }
        }

        public BinData(NodeWrapper node)
        {
            if (node == null)
                return;

            Commercial = (CreditCardCommercial)CollectionUtil.Find(CreditCardCommercial.ALL, node.GetString("commercial"), CreditCardCommercial.UNKNOWN);
            Debit = (CreditCardDebit)CollectionUtil.Find(CreditCardDebit.ALL, node.GetString("debit"), CreditCardDebit.UNKNOWN);
            DurbinRegulated = (CreditCardDurbinRegulated)CollectionUtil.Find(CreditCardDurbinRegulated.ALL, node.GetString("durbin-regulated"), CreditCardDurbinRegulated.UNKNOWN);
            Healthcare = (CreditCardHealthcare)CollectionUtil.Find(CreditCardHealthcare.ALL, node.GetString("healthcare"), CreditCardHealthcare.UNKNOWN);
            Payroll = (CreditCardPayroll)CollectionUtil.Find(CreditCardPayroll.ALL, node.GetString("payroll"), CreditCardPayroll.UNKNOWN);
            Prepaid = (CreditCardPrepaid)CollectionUtil.Find(CreditCardPrepaid.ALL, node.GetString("prepaid"), CreditCardPrepaid.UNKNOWN);
            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
        }

        public BinData(dynamic bin)
        {
            if (bin == null)
                return;

            Commercial = (CreditCardCommercial)CollectionUtil.Find(CreditCardCommercial.ALL, (string)bin.commercial, CreditCardCommercial.UNKNOWN);
            Debit = (CreditCardDebit)CollectionUtil.Find(CreditCardDebit.ALL, (string)bin.debit, CreditCardDebit.UNKNOWN);
            DurbinRegulated = (CreditCardDurbinRegulated)CollectionUtil.Find(CreditCardDurbinRegulated.ALL, (string)bin.durbinRegulated, CreditCardDurbinRegulated.UNKNOWN);
            Healthcare = (CreditCardHealthcare)CollectionUtil.Find(CreditCardHealthcare.ALL, (string)bin.healthcare, CreditCardHealthcare.UNKNOWN);
            Payroll = (CreditCardPayroll)CollectionUtil.Find(CreditCardPayroll.ALL, (string)bin.payroll, CreditCardPayroll.UNKNOWN);
            Prepaid = (CreditCardPrepaid)CollectionUtil.Find(CreditCardPrepaid.ALL, (string)bin.prepaid, CreditCardPrepaid.UNKNOWN);
            _CountryOfIssuance = bin.countryOfIssuance;
            _IssuingBank = bin.issuingBank;
            _ProductId = bin.productId;
        }

        [Obsolete("Mock Use Only")]
        protected internal BinData() { }
    }
}

