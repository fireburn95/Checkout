namespace CheckoutKata.Models
{
    public abstract class AbstractPromotion
    {
        public Product Product { get; set; }
        public int PurchaseQuantity { get; set; }
    }
}