using System.Collections.Generic;

namespace SessionTest.Models
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Products = new List<ProductViewModel>();
        }

        public string Id { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }

        public decimal Total { get; set; }
    }
}
