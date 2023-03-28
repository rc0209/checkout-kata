namespace checkout_kata_tests
{
    using checkout_kata;

    internal class ProductBuilder
    {
        private string sku = string.Empty;
        private int unitPrice = 0;

        internal ProductBuilder WithSku(string value)
        {
            this.sku = value;
            return this;
        }

        internal ProductBuilder WithUnitPrice(int value)
        {
            this.unitPrice = value;
            return this;
        }

        internal Product Build()
        {
            return new Product(this.sku, this.unitPrice);
        }
    }
}