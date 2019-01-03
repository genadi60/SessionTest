using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SessionTest.ViewModels;

namespace SessionTest.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<Product>();
        }

        public string Id { get; set; }

        public decimal Total => Products.Sum(p => p.Price * p.Quantity);

        public virtual ICollection<Product> Products { get; set; }
    }
}
