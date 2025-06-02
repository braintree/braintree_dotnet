using System;

namespace Braintree
{
    public class BinData
    {
        public virtual CreditCardBusiness Business { get; protected set; }
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardConsumer Consumer { get; protected set; }
        public virtual CreditCardCorporate Corporate { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }
        public virtual CreditCardPrepaidReloadable PrepaidReloadable { get; protected set; }
        public virtual CreditCardPurchase Purchase { get; protected set; }

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

            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
            Business = node.GetEnum("business", CreditCardBusiness.UNKNOWN);
            Commercial = node.GetEnum("commercial", CreditCardCommercial.UNKNOWN);
            Consumer = node.GetEnum("consumer", CreditCardConsumer.UNKNOWN);
            Corporate = node.GetEnum("corporate", CreditCardCorporate.UNKNOWN);
            Debit = node.GetEnum("debit", CreditCardDebit.UNKNOWN);
            DurbinRegulated = node.GetEnum("durbin-regulated", CreditCardDurbinRegulated.UNKNOWN);
            Healthcare = node.GetEnum("healthcare", CreditCardHealthcare.UNKNOWN);
            Payroll = node.GetEnum("payroll", CreditCardPayroll.UNKNOWN);
            Prepaid = node.GetEnum("prepaid", CreditCardPrepaid.UNKNOWN);
            PrepaidReloadable = node.GetEnum("prepaid-reloadable", CreditCardPrepaidReloadable.UNKNOWN);
            Purchase = node.GetEnum("purchase", CreditCardPurchase.UNKNOWN);
        }

        public BinData(dynamic bin)
        {
            if (bin == null)
                return;

            _CountryOfIssuance = bin.countryOfIssuance;
            _IssuingBank = bin.issuingBank;
            _ProductId = bin.productId;
            Business = EnumHelper.FindEnum((string)bin.business, CreditCardBusiness.UNKNOWN);
            Commercial = EnumHelper.FindEnum((string)bin.commercial, CreditCardCommercial.UNKNOWN);
            Consumer = EnumHelper.FindEnum((string)bin.consumer, CreditCardConsumer.UNKNOWN);
            Corporate = EnumHelper.FindEnum((string)bin.corporate, CreditCardCorporate.UNKNOWN);
            Debit = EnumHelper.FindEnum((string)bin.debit, CreditCardDebit.UNKNOWN);
            DurbinRegulated = EnumHelper.FindEnum((string)bin.durbinRegulated, CreditCardDurbinRegulated.UNKNOWN);
            Healthcare = EnumHelper.FindEnum((string)bin.healthcare, CreditCardHealthcare.UNKNOWN);
            Payroll = EnumHelper.FindEnum((string)bin.payroll, CreditCardPayroll.UNKNOWN);
            Prepaid = EnumHelper.FindEnum((string)bin.prepaid, CreditCardPrepaid.UNKNOWN);
            PrepaidReloadable = EnumHelper.FindEnum((string)bin.prepaidReloadable, CreditCardPrepaidReloadable.UNKNOWN);
            Purchase = EnumHelper.FindEnum((string)bin.purchase, CreditCardPurchase.UNKNOWN);
        }

        [Obsolete("Mock Use Only")]
        protected internal BinData() { }
    }
}

