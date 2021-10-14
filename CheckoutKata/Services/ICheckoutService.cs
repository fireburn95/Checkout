namespace CheckoutKata.Models
{
    public interface ICheckoutService
    {
        public void AddProduct(Product product);

        public void RemoveProduct(Product product);

        public void ClearProducts(Product product);

        public void ClearBasket();

        public int CompleteOrder();
    }
}