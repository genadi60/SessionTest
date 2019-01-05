using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
