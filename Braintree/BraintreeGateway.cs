using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class BraintreeGateway
    {
        public Environment Environment
        {
            get { return Configuration.Environment; }
            set { Configuration.Environment = value; }
        }

        public String MerchantID
        {
            get { return Configuration.MerchantID; }
            set { Configuration.MerchantID = value; }
        }

        public String PublicKey
        {
            get { return Configuration.PublicKey; }
            set { Configuration.PublicKey = value; }
        }

        public String PrivateKey
        {
            get { return Configuration.PrivateKey; }
            set { Configuration.PrivateKey = value; }
        }

        public CustomerGateway Customer
        {
            get { return new CustomerGateway(); }
        }

        public AddressGateway Address
        {
            get { return new AddressGateway(); }
        }

        public CreditCardGateway CreditCard
        {
            get { return new CreditCardGateway(); }
        }

        public TransactionGateway Transaction
        {
            get { return new TransactionGateway(); }
        }


        public String TrData(Request trData, String redirectURL)
        {
            return TrUtil.BuildTrData(trData, redirectURL);
        }
    }
}
