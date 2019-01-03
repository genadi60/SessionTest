using System.Collections.Generic;

namespace SessionTest.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Products = new List<ProductViewModel>();
        }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
