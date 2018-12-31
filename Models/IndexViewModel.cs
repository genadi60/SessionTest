using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Models
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
