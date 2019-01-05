﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SessionTest.ViewModels;

namespace SessionTest.Models
{
    public class Package
    {
        public string Id { get; set; }

        public decimal Amount => Orders.Sum(o => o.Total);

        public virtual ICollection<Order> Orders{ get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Active;

        public PackageStatus PackageStatus { get; set; } = PackageStatus.Pending;

        public string ClientId { get; set; }
        public virtual IdentityUser Client { get; set; }

        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ShippingData ShippingData { get; set; }
    }
}
