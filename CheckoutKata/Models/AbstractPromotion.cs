namespace CheckoutKata.Models
{
    public abstract class AbstractPromotion
    {
        public string ProductSku { get; init; }
        public int PurchaseQuantity { get; init; }
    }
}