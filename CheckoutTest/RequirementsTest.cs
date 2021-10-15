using System;
using System.Collections.Generic;
using CheckoutKata.Models;
using CheckoutKata.Services;
using Xunit;

namespace CheckoutTest
{
    public class RequirementsTest
    {
        private readonly ICheckoutService _checkoutService;

        public RequirementsTest()
        {
            _checkoutService = new CheckoutService();
        }
        
        [Fact]
        public void TestOne()
        {
            // Given
            var productA = new Product { Sku = "A", Price = 10.0m };
            
            // When
            _checkoutService.AddItem(productA);

            // Then
            var basket = _checkoutService.GetCurrentBasket();
            // Basket has one product
            Assert.Single(basket);
            
            // The item is the product
            Assert.Equal(productA, basket[0].Product);
            
            // There is just one item
            Assert.Equal(1, basket[0].Quantity);
        }
        
        [Fact]
        public void TestTwo()
        {
            // Given
            var productA = new Product{ Sku = "A", Price = 10.0m };
            var productB = new Product{ Sku = "B", Price = 15.0m };

            // Add to checkout
            _checkoutService.AddItem(productA);
            _checkoutService.AddItem(productA);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder(new List<AbstractPromotion>());

            // Calculate total (20 + 30)
            Assert.Equal(50m, totalPrice);
        }
        
        [Fact]
        public void TestThree()
        {
            // Given
            var productB = new Product{ Sku = "B", Price = 15.0m };
            
            // Create two Promotions
            var buy3ProdBFor40 = new BuyQuantityForPrice() { Product = productB, PurchaseQuantity = 3, NewPrice = 40};

            // Add 8 units to checkout
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            _checkoutService.AddItem(productB);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder(new List<AbstractPromotion>(){ buy3ProdBFor40 });

            // Calculate total (40 + 40 + 15 + 15)
            Assert.Equal(110m, totalPrice);
        }
        
        [Fact]
        public void TestFour()
        {
            // Given
            var productD = new Product{ Sku = "D", Price = 55.0m };
            
            // Create two Promotions
            var buy2ProdDGet25Off = new BuyQuantityGetPercentOff() { Product = productD, PurchaseQuantity = 2, PercentDiscount = 25f};

            // Add 5 units to checkout
            _checkoutService.AddItem(productD);
            _checkoutService.AddItem(productD);
            _checkoutService.AddItem(productD);
            _checkoutService.AddItem(productD);
            _checkoutService.AddItem(productD);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder(new List<AbstractPromotion>() { buy2ProdDGet25Off });

            // Calculate total (82.5 + 82.5 + 55)
            Assert.Equal(220m, totalPrice);
        }
    }
}