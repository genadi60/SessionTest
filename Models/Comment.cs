using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SessionTest.Models
{
    public class Comment 
    {
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }
        
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
