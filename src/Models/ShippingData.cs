﻿using SessionTest.InputModels;
using SessionTest.MappingServices;

namespace SessionTest.Models
{
    public class ShippingData : IMapFrom<ShippingDataInputModel>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
