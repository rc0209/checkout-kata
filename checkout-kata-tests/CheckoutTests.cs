namespace checkout_kata_tests
{
    using checkout_kata;
    using checkout_kata_contracts;
    using FluentAssertions;

    using Xunit;

    public class CheckoutTests
    {
        [Fact]
        public void ScanningProductWhenNoProductsAvailableThrowsException()
        {
            var badSku = "X";

            var sut = new Checkout();

            var act = () => sut.Scan(badSku);

            act.Should().Throw<MissingDataException>().WithMessage($"Product with Sku '{badSku}' not found");
        }

        [Fact]
        public void ScanningUnknownProductThrowsException()
        {
            var sku = "A";
            var badSku = "X";
            var unitPrice = 50;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).Build());

            var act = () => sut.Scan(badSku);

            act.Should().Throw<MissingDataException>().WithMessage($"Product with Sku '{badSku}' not found");
        }

        [Fact]
        public void ScanningProductGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).Build());

            sut.Scan(sku);
            sut.GetTotalPrice().Should().Be(unitPrice);
        }

        [Fact]
        public void ScanningSameProductMultipleTimesGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).Build());

            sut.Scan(sku);
            sut.Scan(sku);

            sut.GetTotalPrice().Should().Be(unitPrice * 2);
        }

        [Fact]
        public void ScanningMultipleItemsGivesCorrectPrice()
        {
            var skuA = "A";
            var unitPriceA = 50;
            var skuB = "B";
            var unitPriceB = 30;

            var sut = new Checkout(new ProductBuilder().WithSku(skuA).WithUnitPrice(unitPriceA).Build(),
                new ProductBuilder().WithSku(skuB).WithUnitPrice(unitPriceB).Build());

            sut.Scan(skuA);
            sut.Scan(skuB);

            sut.GetTotalPrice().Should().Be(unitPriceA + unitPriceB);
        }

        [Fact]
        public void ScanningMultipleItemsInAnyOrderGivesCorrectPrice()
        {
            var skuA = "A";
            var unitPriceA = 50;
            var skuB = "B";
            var unitPriceB = 30;

            var sut = new Checkout(new ProductBuilder().WithSku(skuA).WithUnitPrice(unitPriceA).Build(),
                new ProductBuilder().WithSku(skuB).WithUnitPrice(unitPriceB).Build());

            sut.Scan(skuB);
            sut.Scan(skuA);
            sut.Scan(skuB);

            sut.GetTotalPrice().Should().Be(unitPriceB + unitPriceA + unitPriceB);
        }
    }
}