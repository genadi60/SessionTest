using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class CreateCategoryInputModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
