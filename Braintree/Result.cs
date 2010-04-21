#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Result<T> where T : class
    {
        public CreditCardVerification CreditCardVerification { get; protected set; }
        public Transaction Transaction { get; protected set; }
        public ValidationErrors Errors { get; protected set; }
        public Dictionary<String, String> Parameters { get; protected set; }
        public T Target { get; protected set; }

        public Result(NodeWrapper node)
        {
            if (node.IsSuccess())
            {
                Target = newInstanceFromResponse(node);
            }
            else
            {
                Errors = new ValidationErrors(node);
                NodeWrapper verificationNode = node.GetNode("verification");
                if (verificationNode != null) {
                    CreditCardVerification = new CreditCardVerification(verificationNode);
                }

                NodeWrapper transactionNode = node.GetNode("transaction");
                if (transactionNode != null)
                {
                    Transaction = new Transaction(transactionNode);
                }
                Parameters = node.GetNode("params").GetFormParameters();
            }
        }

        public virtual Boolean IsSuccess()
        {
            return Errors == null;
        }

        private T newInstanceFromResponse(NodeWrapper node)
        {
            if (typeof(T) == typeof(Address))
            {
                return new Address(node) as T;
            }
            else if (typeof(T) == typeof(CreditCard))
            {
                return new CreditCard(node) as T;
            }
            else if (typeof(T) == typeof(Customer))
            {
                return new Customer(node) as T;
            }
            else if (typeof(T) == typeof(Transaction))
            {
                return new Transaction(node) as T;
            }
            else if (typeof(T) == typeof(Subscription))
            {
                return new Subscription(node) as T;
            }

            throw new Exception("Unknown T: " + typeof(T).ToString());
        }
    }
}
