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
    }
}