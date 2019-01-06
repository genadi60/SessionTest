using SessionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SessionTest.InputModels;
using SessionTest.MappingServices;

namespace SessionTest.ViewModels
{
    public class PackageViewModel : IMapFrom<Package>
    {
        public string Id { get; set; }

        public decimal Amount => Orders.Sum(o => o.Total);

        public string CartId { get; set; }
       
        public string PaymentMethod { get; set; }

        public DateTime IssuedOn { get; set; }

        public string ClientId { get; set; }
        public IdentityUser Client { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; }

        public PackageStatus PackageStatus { get; set; }

        public virtual ICollection<OrderViewModel> Orders { get; set; }

        public virtual ShippingDataInputModel ShippingData { get; set; }
    }
}
