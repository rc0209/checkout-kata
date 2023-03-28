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
            var sku1 = "A";
            var unitPrice1 = 50;
            var sku2 = "B";
            var unitPrice2 = 30;

            var sut = new Checkout(new ProductBuilder().WithSku(sku1).WithUnitPrice(unitPrice1).Build(),
                new ProductBuilder().WithSku(sku2).WithUnitPrice(unitPrice2).Build());

            sut.Scan(sku1);
            sut.Scan(sku2);

            sut.GetTotalPrice().Should().Be(unitPrice1 + unitPrice2);
        }
    }
}