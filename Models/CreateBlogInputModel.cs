using System.ComponentModel.DataAnnotations;
using SessionTest.MappingServices;

namespace SessionTest.Models
{
    public class CreateBlogInputModel : IMapTo<Blog>
    {
        [Required]
        [MinLength(6)]
        public string Title { get; set; }

        [Required]
        [MinLength(50)]
        public string Content { get; set; }


    }
}
