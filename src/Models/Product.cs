﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class Product
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Unit { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
