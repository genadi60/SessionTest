using System;
using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class Blog
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [MinLength(50)]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; } = DateTime.UtcNow;

        public string PictureUri { get; set; }
    }
}
