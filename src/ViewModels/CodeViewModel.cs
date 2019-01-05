﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.ViewModels
{
    public class CodeViewModel
    {
        public string Id { get; set; }

        public bool IsAuthorized { get; set; }

        public string Guest { get; set; }

        public string Message { get; set; }
    }
}
