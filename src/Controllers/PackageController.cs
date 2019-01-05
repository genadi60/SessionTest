using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class PackageController : BaseController
    {
        private readonly IPackagesService _packagesService;
        private readonly IPaymentMethodsService _paymentMethodsService;
        private readonly UserManager<IdentityUser> _userManager;

        public PackageController(IPackagesService packagesService, 
            UserManager<IdentityUser> userManager, 
            IPaymentMethodsService paymentMethodsService)
        {
            _packagesService = packagesService;
            _userManager = userManager;
            _paymentMethodsService = paymentMethodsService;
        }

        
        public IActionResult Index(ShippingDataInputModel shipping)
        {
            var model = _packagesService.GetPackage(shipping);
            this.ViewData["Methods"] = _paymentMethodsService.All()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });

            return View(model);
        }

        
        [HttpPost]
        public IActionResult Index(PackageViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            model.ClientId = userId;

            var packageId = _packagesService.ConfirmPackage(HttpContext, model).Result;

            var packageViewModel = _packagesService.GetPackageViewModel(packageId);

            return View("Details", packageViewModel);
        }

        public IActionResult MyPackages()
        {

        }
    }
}
