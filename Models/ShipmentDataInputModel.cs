﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Models
{
    public class ShipmentDataInputModel
    {
        public string Id { get; set; }

        public string CartId { get; set; }
        public virtual CartViewModel Cart { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
