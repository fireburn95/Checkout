using System.Collections.Generic;
using CheckoutKata.Dtos;

namespace CheckoutKata.Models
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
            throw new System.NotImplementedException();
        }
        
        public void AddProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void ClearProducts(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void ClearBasket()
        {
            throw new System.NotImplementedException();
        }

        public decimal CompleteOrder()
        {
            throw new System.NotImplementedException();
        }
    }
}