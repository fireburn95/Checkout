using System.Collections.Generic;
using CheckoutKata.Dtos;
using CheckoutKata.Models;

namespace CheckoutKata.Services
{
    public interface ICheckoutService
    {
        public List<CheckoutItem> GetCurrentBasket();
        
        public void AddProduct(Product product);

        public void RemoveProduct(Product product);

        public void ClearProducts(Product product);

        public void ClearBasket();

        public decimal CompleteOrder();
    }
}