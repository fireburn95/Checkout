using System.Collections.Generic;
using System.Linq;
using CheckoutKata.Dtos;
using CheckoutKata.Models;
using CheckoutKata.Services;

namespace CheckoutKata.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly List<CheckoutItem> _basket;
        public CheckoutService()
        {
            _basket = new List<CheckoutItem>();
        }

        public List<CheckoutItem> GetCurrentBasket()
        {
            return _basket;
        }
        
        public void AddItem(Product product)
        {
            var checkoutItem = _basket.FirstOrDefault(item => item.Product.Equals(product));

            if (checkoutItem is not null)
            {
                var index = _basket.FindIndex(item => item.Product == product);
                _basket[index].Quantity++;
            }
            else
            {
                var newCheckoutItem = new CheckoutItem()
                {
                    Product = product, Quantity = 1
                };
                _basket.Add(newCheckoutItem);
            }
        }

        public void RemoveItem(Product product)
        {
            var checkoutItem = _basket.FirstOrDefault(item => item.Product.Equals(product));

            if (checkoutItem is null) return;

            if (checkoutItem.Quantity == 1)
            {
                _basket.Remove(checkoutItem);
            }

            var index = _basket.FindIndex(item => item.Product == product);
            _basket[index].Quantity--;
        }

        public void ClearProduct(Product product)
        {
            var checkoutItem = _basket.First(item => item.Product.Equals(product));
            _basket.Remove(checkoutItem);
        }

        public void ClearBasket()
        {
            _basket.Clear();
        }

        public decimal CompleteOrder()
        {
            return 152m;
            // todo
        }
    }
}