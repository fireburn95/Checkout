using System;
using System.Collections.Generic;
using System.Linq;
using CheckoutKata.Dtos;
using CheckoutKata.Models;

namespace CheckoutKata.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly List<CheckoutItem> _basket;
        private readonly List<Product> _products;
        
        public CheckoutService()
        {
            
            _basket = new List<CheckoutItem>();

            _products = new List<Product>()
            {
                new() {Sku = "A", Price = 10.0m},
                new() {Sku = "B", Price = 15.0m},
                new() {Sku = "C", Price = 40.0m},
                new() {Sku = "D", Price = 55.0m},
            };
        }

        public List<CheckoutItem> GetCurrentBasket()
        {
            return _basket;
        }
        
        public void AddItem(string sku)
        {
            var checkoutItem = _basket.FirstOrDefault(item => item.Sku.Equals(sku));

            if (checkoutItem is not null)
            {
                var index = _basket.FindIndex(item => item.Sku.Equals(sku));
                _basket[index].Quantity++;
            }
            else
            {
                var newCheckoutItem = new CheckoutItem()
                {
                    Sku = sku, Quantity = 1
                };
                _basket.Add(newCheckoutItem);
            }
        }

        public void RemoveItem(string sku)
        {
            var checkoutItem = _basket.FirstOrDefault(item => item.Sku.Equals(sku));

            if (checkoutItem is null) 
                return;

            if (checkoutItem.Quantity == 1)
            {
                _basket.Remove(checkoutItem);
                return;
            }

            var index = _basket.FindIndex(item => item.Sku.Equals(sku));
            _basket[index].Quantity--;
        }

        public void ClearProduct(string sku)
        {
            var checkoutItem = _basket.First(item => item.Sku.Equals(sku));
            _basket.Remove(checkoutItem);
        }

        public void ClearBasket()
        {
            _basket.Clear();
        }

        public decimal CompleteOrder(List<AbstractPromotion> promotions)
        {
            var totalPrice = 0m;

            // For each item in basket
            _basket.ForEach(item =>
            {
                // Get the product from the store
                var product = _products.First(product => product.Sku == item.Sku);
                
                // Look for a relevant promotion
                var relevantPromotions = promotions.Where(promotion => promotion.ProductSku == item.Sku).ToList();

                // If there are more than one promotions for a product, error
                if (relevantPromotions.Count > 1) 
                    throw new NotSupportedException();

                var applyPromotion = relevantPromotions.FirstOrDefault();

                decimal checkoutItemTotal;
                decimal promotionTotal, nonDiscountedItemsTotal;
                switch (applyPromotion)
                {
                    case BuyQuantityForPrice promo:
                        promotionTotal = (int)(item.Quantity / promo.PurchaseQuantity) * promo.NewPrice;

                        nonDiscountedItemsTotal = (item.Quantity % promo.PurchaseQuantity) * product.Price;

                        checkoutItemTotal = promotionTotal + nonDiscountedItemsTotal;
                        break;
                    case BuyQuantityGetPercentOff promo:
                        var actualPriceOfGroup = (promo.PurchaseQuantity * product.Price) *
                                                 ((100 - (decimal) promo.PercentDiscount) / 100);
                        promotionTotal = (int)(item.Quantity / promo.PurchaseQuantity) * actualPriceOfGroup;
                        
                        nonDiscountedItemsTotal = (item.Quantity % promo.PurchaseQuantity) * product.Price;
                        
                        checkoutItemTotal = promotionTotal + nonDiscountedItemsTotal;
                        break;
                    default:
                        // No promotion on this item, or unhandled promotion type in which case ignore
                        checkoutItemTotal = item.Quantity * product.Price;
                        break;
                }

                totalPrice += checkoutItemTotal;
            });
            return totalPrice;
        }
    }
}