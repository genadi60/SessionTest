using System.Collections.Generic;
using System.Linq;

namespace SessionTest.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<ProductViewModel>();
        }

        public string Id { get; set; }

        public decimal Total => Products.Sum(p => p.Price * p.Quantity);

        public virtual ICollection<ProductViewModel> Products { get; set; }
    }
}
