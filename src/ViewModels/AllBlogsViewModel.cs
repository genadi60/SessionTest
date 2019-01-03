using System.Collections.Generic;

namespace SessionTest.ViewModels
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
