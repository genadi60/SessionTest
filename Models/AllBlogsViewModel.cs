using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Models
{
    public class AllBlogsViewModel
    {
        public AllBlogsViewModel()
        {
            Blogs = new List<BlogViewModel>();
        }

        public IEnumerable<BlogViewModel> Blogs { get; set; }
    }
}
