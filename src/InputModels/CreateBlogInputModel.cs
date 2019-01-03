using System.ComponentModel.DataAnnotations;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.InputModels
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
