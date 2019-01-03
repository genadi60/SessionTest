using System;
using SessionTest.InputModels;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class BlogViewModel : IMapFrom<Blog>, IMapFrom<CreateBlogInputModel>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string PictureUri { get; set; }
    }
}
