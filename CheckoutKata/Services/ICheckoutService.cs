using System.Collections.Generic;
using CheckoutKata.Dtos;
using CheckoutKata.Models;

namespace CheckoutKata.Services
{
    public interface ICheckoutService
    {
        public List<CheckoutItem> GetCurrentBasket();
        
        public void AddItem(string sku);

        public void RemoveItem(string sku);

        public void ClearProduct(string sku);

        public void ClearBasket();

        public decimal CompleteOrder(List<AbstractPromotion> promotions);
    }
}