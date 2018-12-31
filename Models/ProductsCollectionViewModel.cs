using System.Collections.Generic;

namespace SessionTest.Models
{
    public class ProductsCollectionViewModel
    {
        public virtual ICollection<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
