﻿namespace checkout_kata
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using checkout_kata_contracts;

    public class Checkout : ICheckout
    {
        private readonly IReadOnlyDictionary<string, Product> products;
        private readonly ConcurrentDictionary<string, BasketItem> items;

        public Checkout(params Product[] products)
        {
            this.products = products.ToDictionary(k => k.Sku, v => v);
            this.items = new ConcurrentDictionary<string, BasketItem>();
        }

        public void Scan(string item)
        {
            if (!this.products.ContainsKey(item))
            {
                throw new MissingDataException(item);
            }

            var product = this.products[item];

            this.items.AddOrUpdate(item, new BasketItem(1, product),
                (s, basketItem) => basketItem with {Quantity = basketItem.Quantity + 1});
        }

        public int GetTotalPrice()
        {
            return this.items.Sum(i => i.Value.TotalPrice());
        }

        private record BasketItem(int Quantity, Product Product)
        {
            internal int TotalPrice()
            {
                return this.Quantity * this.Product.UnitPrice;
            }
        }
    }
}