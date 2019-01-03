using System.ComponentModel.DataAnnotations;

namespace SessionTest.InputModels
{
    public class CreateCategoryInputModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
