using System.ComponentModel.DataAnnotations;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.InputModels
{
    public class EditProductInputModel : IMapFrom<ProductViewModel>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MinLength(15)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string Image { get; set; }
    }
}
