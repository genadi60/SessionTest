using System;

namespace SessionTest.Models
{
    public class Payment
    {
        public string Id { get; set; }

        public decimal Cost { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Active;

        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public DateTime PaidOn { get; set; } = DateTime.UtcNow;
    }
}
