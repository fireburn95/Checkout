namespace CheckoutKata.Models
{
    public abstract class AbstractPromotion
    {
        public Product Product { get; set; }
        private int PurchaseQuantity { get; set; }
    }
}