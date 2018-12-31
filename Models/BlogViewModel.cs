using SessionTest.MappingServices;
using System;

namespace SessionTest.Models
{
    public class BlogViewModel : IMapFrom<Blog>, IMapFrom<CreateBlogInputModel>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string PictureUri { get; set; }
    }
}
