namespace checkout_kata_tests
{
    using System.Collections.Generic;

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

        [Fact]
        public void ScanningSingleItemWithSpecialOfferGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;
            var qty = 3;
            var offerPrice = 130;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).WithSpecialOffer(qty, offerPrice).Build());

            sut.Scan(sku);

            sut.GetTotalPrice().Should().Be(unitPrice);
        }

        [Fact]
        public void ScanningItemMatchingSpecialOfferGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;
            var qty = 3;
            var offerPrice = 130;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).WithSpecialOffer(qty, offerPrice).Build());

            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);

            sut.GetTotalPrice().Should().Be(offerPrice);
        }

        [Fact]
        public void ScanningItemOverMatchingSpecialOfferGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;
            var qty = 3;
            var offerPrice = 130;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).WithSpecialOffer(qty, offerPrice).Build());

            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);

            sut.GetTotalPrice().Should().Be(offerPrice + unitPrice);
        }

        [Fact]
        public void ScanningItemMatchingSpecialOfferMultipleTimesGivesCorrectPrice()
        {
            var sku = "A";
            var unitPrice = 50;
            var qty = 3;
            var offerPrice = 130;

            var sut = new Checkout(new ProductBuilder().WithSku(sku).WithUnitPrice(unitPrice).WithSpecialOffer(qty, offerPrice).Build());

            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);
            sut.Scan(sku);

            sut.GetTotalPrice().Should().Be(offerPrice * 2);
        }

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void ScanningMultipleItemsMultipleTimesWithSpecialsGivesCorrectPrice(string[] itemsToScan, int expectedTotal)
        {
            var sut = GetCheckoutManagerWithFullProductCatalog();

            foreach (var item in itemsToScan)
            {
                sut.Scan(item);
            }

            sut.GetTotalPrice().Should().Be(expectedTotal);
        }

        public static IEnumerable<object[]> ProductTestData => new List<object[]>
        {
            new object[] {new List<string> {"A", "B", "A", "D", "A"}, 175},
            new object[] {new List<string> {"A", "B", "A", "B", "C"}, 165},
            new object[] {new List<string> {"A", "B", "C", "D"}, 115},
            new object[] {new List<string> {"D", "C", "B", "A"}, 115}
        };

        private static Checkout GetCheckoutManagerWithFullProductCatalog()
        {
            return new Checkout(
                new ProductBuilder().WithSku("A").WithUnitPrice(50).WithSpecialOffer(3, 130).Build(),
                new ProductBuilder().WithSku("B").WithUnitPrice(30).WithSpecialOffer(2, 45).Build(),
                new ProductBuilder().WithSku("C").WithUnitPrice(20).Build(),
                new ProductBuilder().WithSku("D").WithUnitPrice(15).Build()
            );
        }
    }
}