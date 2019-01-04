﻿using System;

namespace SessionTest.Models
{
    public class Payment
    {
        public string Id { get; set; }

        public decimal Cost => Cart.Total;

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Active;

        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public string ShippingDataId { get; set; }
        public virtual ShippingData ShippingData { get; set; }

        public DateTime PaidOn { get; set; } = DateTime.UtcNow;
    }
}
