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

        public IActionResult Packages()
        {
            var model = _packagesService.All();

            return View(model);
        }

        [HttpPost]
        public IActionResult PackageDetails(string search)
        {
            return RedirectToAction("ConfirmDetails", new { id = search });
        }

        [HttpPost]
        public IActionResult Details(string id)
        {
            return RedirectToAction("ConfirmDetails", new {id = id});
        }

        public IActionResult ConfirmDetails(string id)
        {
            PackageViewModel package = null;
            if (id != null)
            {
                package = _packagesService.GetPackageViewModel(id);
            }
            
            if (package == null || id == null)
            {
                return View("Invalid");
            }

            return View("Details", package);
        }



        [HttpPost]
        public IActionResult Deliver(string id)
        {
            return RedirectToAction("ConfirmDeliver", new {id = id});
        }

        public IActionResult ConfirmDeliver(string id)
        {
            _packagesService.ConfirmDeliver(id);

            return RedirectToAction("Packages");
        }

        [HttpPost]
        public IActionResult Acquire(string id)
        {
            return RedirectToAction("ConfirmAcquire", new { id = id });
        }

        public IActionResult ConfirmAcquire(string id)
        {
            _packagesService.ConfirmAcquire(id);

            return RedirectToAction("Packages");
        }
    }
}
