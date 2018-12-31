using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class Category 
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
