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
            var product = new Product { Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            
            // When
            _checkoutService.AddItem(product);

            // Then
            var basket = _checkoutService.GetCurrentBasket();
            AssertSingleProductInBasket(product, basket);
        }
        
        [Fact]
        public void TestRemoveProduct()
        {
            // Given
            var product = new Product { Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
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
        public void TestClearProducts()
        {
            // Given
            var product1 = new Product{ Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            var product2 = new Product{ Id = Guid.NewGuid(), Name = "Chocolate", Price = 33.0m };
            
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
            var product1 = new Product{ Id = Guid.NewGuid(), Name = "Cheese", Price = 40.0m };
            var product2 = new Product{ Id = Guid.NewGuid(), Name = "Chocolate", Price = 33.0m };
            var product3 = new Product{ Id = Guid.NewGuid(), Name = "Chips", Price = 18.5m };

            // Create two Promotions todo persist
            var buy2Prod1Get25Off = new BuyQuantityGetPercentOff();
            var buy2Prod2For55 = new BuyQuantityForPrice();

            // Add to checkout
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product1);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product2);
            _checkoutService.AddItem(product3);
            _checkoutService.AddItem(product3);
            
            // When
            var totalPrice = _checkoutService.CompleteOrder();

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