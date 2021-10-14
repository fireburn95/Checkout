using System.Collections.Generic;
using CheckoutKata.Models;

namespace CheckoutKata.Dtos
{
    public class CheckoutItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}