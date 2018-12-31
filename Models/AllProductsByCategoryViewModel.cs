using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Models
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
