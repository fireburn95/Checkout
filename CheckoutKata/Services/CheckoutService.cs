using System;
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

            if (checkoutItem is null) 
                return;

            if (checkoutItem.Quantity == 1)
            {
                _basket.Remove(checkoutItem);
                return;
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
            throw new NotImplementedException();
        }

        public decimal CompleteOrder(List<AbstractPromotion> promotions)
        {
            var totalPrice = 0m;

            // Apply each promotion
            _basket.ForEach(item =>
            {
                // Look for a relevant promotion
                var relevantPromotions = promotions.Where(promotion => promotion.Product == item.Product).ToList();

                // If there are more than one promotions for a product, error
                if (relevantPromotions.Count > 1) 
                    throw new NotSupportedException();

                var applyPromotion = relevantPromotions.FirstOrDefault();

                decimal checkoutItemTotal;
                int quantityInBasket, quantityOfPromotion, numberOfTimesApplied;
                decimal promotionTotal, nonDiscountedItemsTotal;
                switch (applyPromotion)
                {
                    case BuyQuantityForPrice promo:
                        quantityInBasket = item.Quantity;
                        quantityOfPromotion = promo.PurchaseQuantity;

                        numberOfTimesApplied = quantityInBasket / quantityOfPromotion;
                        promotionTotal = numberOfTimesApplied * promo.NewPrice;

                        nonDiscountedItemsTotal = (quantityInBasket % quantityOfPromotion) * item.Product.Price;

                        checkoutItemTotal = promotionTotal + nonDiscountedItemsTotal;
                        break;
                    case BuyQuantityGetPercentOff promo:
                        quantityInBasket = item.Quantity;
                        quantityOfPromotion = promo.PurchaseQuantity;
                        
                        numberOfTimesApplied = quantityInBasket / quantityOfPromotion;
                        var actualPriceOfGroup = (promo.PurchaseQuantity * item.Product.Price) *
                                                     ((100 - (decimal) promo.PercentDiscount) / 100);
                        promotionTotal = numberOfTimesApplied * actualPriceOfGroup;
                        
                        nonDiscountedItemsTotal = (quantityInBasket % quantityOfPromotion) * item.Product.Price;
                        
                        checkoutItemTotal = promotionTotal + nonDiscountedItemsTotal;
                        break;
                    default:
                        // No promotion on this item, or unhandled promotion type in which case ignore
                        quantityInBasket = item.Quantity;
                        checkoutItemTotal = quantityInBasket * item.Product.Price;
                        break;
                }

                totalPrice += checkoutItemTotal;
            });
            return totalPrice;
        }
    }
}