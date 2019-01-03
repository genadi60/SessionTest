using System.Collections.Generic;

namespace SessionTest.ViewModels
{
    public class ProductsCollectionViewModel
    {
        public virtual ICollection<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
