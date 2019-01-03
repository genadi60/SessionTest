using System.Collections.Generic;
using System.Linq;

namespace SessionTest.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Products = new List<ProductViewModel>();
        }

        public string Id { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }

        public decimal Total => Products.Sum(p => p.Price * p.Quantity);
    }
}
