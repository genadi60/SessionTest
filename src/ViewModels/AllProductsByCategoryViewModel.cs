using System.Collections.Generic;

namespace SessionTest.ViewModels
{
    public class AllProductsByCategoryViewModel
    {
        public AllProductsByCategoryViewModel()
        {
            Products = new List<ProductViewModel>();
        }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
