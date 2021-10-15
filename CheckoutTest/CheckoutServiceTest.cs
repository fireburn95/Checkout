using System;
using System.Collections.Generic;
using System.Linq;
using CheckoutKata.Dtos;
using CheckoutKata.Models;
using CheckoutKata.Services;
using Xunit;

namespace CheckoutTest
{
    public class CheckoutServiceTest
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutServiceTest()
        {
            _checkoutService = new CheckoutService();
        }
        
        [Fact]
        public void TestAddProduct()
        {
            // Given
            var product = new Product { Sku = "Cheese", Price = 40.0m };
            
            // When
            _checkoutService.AddItem(product);

            // Then
            var basket = _checkoutService.GetCurrentBasket();
            AssertSingleProductInBasket(product, basket);
        }
        
        [Fact]
        public void TestRemoveProductCompletely()
        {
            // Given
            var product = new Product { Sku = "Cheese", Price = 40.0m };
            _checkoutService.AddItem(product);
            var basket = _checkoutService.GetCurrentBasket();
            AssertSingleProductInBasket(product, basket);
            
            // When
            _checkoutService.RemoveItem(product);
            
            // Then
            // Basket is empty
            Assert.Empty(_checkoutService.GetCurrentBasket());
        }
        
        [Fact]
        public void TestRemoveProductQuantity()
        {
            // Given
            var product = new Product { Sku = "Cheese", Price = 40.0m };
            _checkoutService.AddItem(product);
            _checkoutService.AddItem(product);
            var basket = _checkoutService.GetCurrentBasket();
            Assert.Single(basket);
            Assert.Equal(product, basket[0].Product);
            Assert.Equal(2, basket[0].Quantity);
            
            // When
            _checkoutService.RemoveItem(product);
            
            // Then
            Assert.Single(basket);
            Assert.Equal(product, basket[0].Product);
            Assert.Equal(1, basket[0].Quantity);
        }
        
        [Fact]
        public void TestClearProducts()
        {
            // Given
            var product1 = new Product{ Sku = "Cheese", Price = 40.0m };
            var product2 = new Product{ Sku = "Chocolate", Price = 33.0m };
            
            // 3 x Product 1, 2 x Product 2
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product2);

            var totalQuantityBefore = _checkoutService.GetCurrentBasket().Select(item => item.Quantity).Sum();
            var product1QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product1)).ToList()[0].Quantity;
            var product2QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product2)).ToList()[0].Quantity;
            Assert.Equal(5, totalQuantityBefore);
            Assert.Equal(3, product1QuantityBefore);
            Assert.Equal(2, product2QuantityBefore);
            
            // When
            _checkoutService.ClearProduct(product1);
            
            // Then
            var totalQuantityAfter = _checkoutService.GetCurrentBasket().Select(item => item.Quantity).Sum();
            var product1After = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product1)).ToList();
            var product2QuantityAfter = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product2)).ToList()[0].Quantity;
            Assert.Equal(2, totalQuantityAfter);
            Assert.Empty(product1After);
            Assert.Equal(2, product2QuantityAfter);
        }
        
        [Fact]
        public void TestClearBaskets()
        {
            // Given
            var product1 = new Product{ Sku = "Cheese", Price = 40.0m };
            var product2 = new Product{ Sku = "Chocolate", Price = 33.0m };
            
            // 1 x Product 1, 2 x Product 2
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product2);
            
            var totalQuantityBefore = _checkoutService.GetCurrentBasket().Select(item => item.Quantity).Sum();
            var product1QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product1)).ToList()[0].Quantity;
            var product2QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product2)).ToList()[0].Quantity;
            Assert.Equal(3, totalQuantityBefore);
            Assert.Equal(1, product1QuantityBefore);
            Assert.Equal(2, product2QuantityBefore);
            
            // When
            _checkoutService.ClearBasket();
            
            // Then
            Assert.Empty(_checkoutService.GetCurrentBasket());
        }
        
        [Fact]
        public void TestCompleteOrder()
        {
            // Given
            var product1 = new Product{ Sku = "Cheese", Price = 40.0m };
            var product2 = new Product{ Sku = "Chocolate", Price = 33.0m };
            var product3 = new Product{ Sku = "Chips", Price = 18.5m };

            // Create two Promotions
            var buy2Prod1Get25Off = new BuyQuantityGetPercentOff() { Product = product1, PercentDiscount = 25.0f, PurchaseQuantity = 2 };
            var buy2Prod2For55 = new BuyQuantityForPrice() { Product = product2, NewPrice = 55, PurchaseQuantity = 2 };
            var promotions = new List<AbstractPromotion>() { buy2Prod1Get25Off, buy2Prod2For55 };

            // Add to checkout
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product3);
            _checkoutService.AddItem(product3);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder(promotions);

            // Calculate total (60 + 55 + 37)
            Assert.Equal(152m, totalPrice);
        }

        private static void AssertSingleProductInBasket(Product product, IReadOnlyList<CheckoutItem> basket)
        {
            // Basket has one product
            Assert.Single(basket);
            
            // The item is the product
            Assert.Equal(product, basket[0].Product);
            
            // There is just one item
            Assert.Equal(1, basket[0].Quantity);
        }
    }
}