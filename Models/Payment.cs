using System;

namespace SessionTest.Models
{
    public class Payment
    {
        public string Id { get; set; }

        public decimal Cost { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Active;

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PaidOn { get; set; }
    }
}
