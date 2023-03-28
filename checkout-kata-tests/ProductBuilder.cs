namespace checkout_kata_tests
{
    using checkout_kata;

    internal class ProductBuilder
    {
        private string sku = string.Empty;
        private int unitPrice = 0;
        private int? specialOfferQuantity;
        private int? specialOfferPrice;

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

        internal ProductBuilder WithSpecialOffer(int qty, int price)
        {
            this.specialOfferQuantity = qty;
            this.specialOfferPrice = price;
            return this;
        }

        internal Product Build()
        {
            return new Product(this.sku, this.unitPrice,
                this.specialOfferQuantity != null && this.specialOfferPrice != null
                    ? new SpecialOffer(this.specialOfferQuantity.Value, this.specialOfferPrice.Value)
                    : null);
        }
    }
}