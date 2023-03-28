﻿namespace checkout_kata
{
    using System.Collections.Generic;
    using System.Linq;
    using checkout_kata_contracts;

    public class Checkout : ICheckout
    {
        private readonly IReadOnlyDictionary<string, Product> products;

        public Checkout(params Product[] products)
        {
            this.products = products.ToDictionary(k => k.Sku, v => v);
        }

        public void Scan(string item)
        {
            if (!this.products.ContainsKey(item))
            {
                throw new MissingDataException(item);
            }
        }

        public int GetTotalPrice()
        {
            throw new System.NotImplementedException();
        }
    }
}