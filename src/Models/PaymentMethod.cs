using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionTest.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            Payments = new List<Payment>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
