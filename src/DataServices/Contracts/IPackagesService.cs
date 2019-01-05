using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.InputModels;
using SessionTest.ViewModels;

namespace SessionTest.DataServices.Contracts
{
    public interface IPackagesService
    {
        PackageInputModel GetPackage(ShippingDataInputModel shipping);

        PackageViewModel GetPackageViewModel(string id);

        Task<string> ConfirmPackage(HttpContext context, PackageViewModel model);
    }
}
