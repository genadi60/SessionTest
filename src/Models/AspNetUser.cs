using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SessionTest.Models
{
    public class AspNetUser : IdentityUser
    {
        [Required]
        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}
