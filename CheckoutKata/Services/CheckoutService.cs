using CheckoutKata.Dtos;

namespace CheckoutKata.Models
{
    public class CheckoutService : ICheckoutService
    {
        private readonly Basket _basket;
        public CheckoutService()
        {
            _basket = new Basket();
        }

        public Basket GetCurrentBasket()
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

        public int CompleteOrder()
        {
            throw new System.NotImplementedException();
        }
    }
}