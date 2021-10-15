using System;
using System.Collections.Generic;
using System.Linq;
using CheckoutKata.Dtos;
using CheckoutKata.Models;
using Xunit;

namespace CheckoutTest
{
    public class CheckoutServiceTest
    {
        private CheckoutService _checkoutService;

        public CheckoutServiceTest()
        {
            _checkoutService = new CheckoutService();
        }
        [Fact]
        public void TestAddProduct()
        {
            // Given
            var product = new Product { Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            
            // When
            _checkoutService.AddProduct(product);

            // Then
            var basket = _checkoutService.GetCurrentBasket();
            AssertSingleProductInBasket(product, basket);
        }
        
        [Fact]
        public void TestRemoveProduct()
        {
            // Given
            var product = new Product { Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            _checkoutService.AddProduct(product);
            var basket = _checkoutService.GetCurrentBasket();
            AssertSingleProductInBasket(product, basket);
            
            // When
            _checkoutService.RemoveProduct(product);
            
            // Then
            // Basket is empty
            Assert.Empty(_checkoutService.GetCurrentBasket());
        }
        
        [Fact]
        public void TestClearProducts()
        {
            // Given
            var product1 = new Product{ Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            var product2 = new Product{ Id = Guid.NewGuid(), Name = "Chocolate", Price = 33.0m };
            
            // 3 x Product 1, 2 x Product 2
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product2);
            _checkoutService.AddProduct(product2);

            var totalQuantityBefore = _checkoutService.GetCurrentBasket().Select(item => item.Quantity).Sum();
            var product1QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product1)).ToList()[0].Quantity;
            var product2QuantityBefore = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product2)).ToList()[0].Quantity;
            Assert.Equal(5, totalQuantityBefore);
            Assert.Equal(3, product1QuantityBefore);
            Assert.Equal(2, product2QuantityBefore);
            
            // When
            _checkoutService.ClearProducts(product1);
            
            // Then
            var totalQuantityAfter = _checkoutService.GetCurrentBasket().Select(item => item.Quantity).Sum();
            var product1QuantityAfter = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product1)).ToList()[0].Quantity;
            var product2QuantityAfter = _checkoutService.GetCurrentBasket().Where(item => item.Product.Equals(product2)).ToList()[0].Quantity;
            Assert.Equal(2, totalQuantityAfter);
            Assert.Equal(0, product1QuantityAfter);
            Assert.Equal(2, product2QuantityAfter);
        }
        
        [Fact]
        public void TestClearBaskets()
        {
            // Given
            var product1 = new Product{ Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            var product2 = new Product{ Id = Guid.NewGuid(), Name = "Chocolate", Price = 33.0m };
            
            // 1 x Product 1, 2 x Product 2
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product2);
            _checkoutService.AddProduct(product2);
            
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
            var product1 = new Product{ Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            var product2 = new Product{ Id = Guid.NewGuid(), Name = "Chocolate", Price = 33.0m };
            var product3 = new Product{ Id = Guid.NewGuid(), Name = "Chips", Price = 18.5m };

            // todo Create two Promotions
            // todo product1 is buy 2 get 25% off
            // todo product2 is buy 2 for 55.0

            // Add to checkout
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product1);
            _checkoutService.AddProduct(product2);
            _checkoutService.AddProduct(product2);
            _checkoutService.AddProduct(product3);
            _checkoutService.AddProduct(product3);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder();

            // Calculate total (60 + 55 + 37)
            Assert.Equal(152m, totalPrice);
        }

        private static void AssertSingleProductInBasket(Product product, List<CheckoutItem> basket)
        {
            // Basket has one item
            Assert.Single(basket);
            
            // The item is the product
            Assert.Equal(product, basket[0].Product);
        }
    }
}