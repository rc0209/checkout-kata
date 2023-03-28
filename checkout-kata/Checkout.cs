namespace checkout_kata
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using checkout_kata_contracts;

    public class Checkout : ICheckout
    {
        private readonly IReadOnlyDictionary<string, Product> products;
        private readonly ConcurrentBag<Product> items;

        public Checkout(params Product[] products)
        {
            this.products = products.ToDictionary(k => k.Sku, v => v);
            this.items = new ConcurrentBag<Product>();
        }

        public void Scan(string item)
        {
            if (!this.products.ContainsKey(item))
            {
                throw new MissingDataException(item);
            }

            this.items.Add(this.products[item]);
        }

        public int GetTotalPrice()
        {
            return this.items.Sum(i => i.UnitPrice);
        }
    }
}