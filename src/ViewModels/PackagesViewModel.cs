using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class AllPackagesViewModel
    {
        public ICollection<PackageViewModel> Packages { get; set; } = new List<PackageViewModel>();
    }
}
