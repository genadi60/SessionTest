using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    
    public class Image
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
