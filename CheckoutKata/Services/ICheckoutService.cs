using System.Collections.Generic;
using CheckoutKata.Dtos;
using CheckoutKata.Models;

namespace CheckoutKata.Services
{
    public interface ICheckoutService
    {
        public List<CheckoutItem> GetCurrentBasket();
        
        public void AddItem(Product product);

        public void RemoveItem(Product product);

        public void ClearProduct(Product product);

        public void ClearBasket();

        public decimal CompleteOrder(List<AbstractPromotion> promotions);
    }
}