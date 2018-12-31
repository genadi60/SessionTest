using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;

namespace SessionTest.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult AllProductFromCategory(int id)
        {
            this.ViewData["Categories"] = this.categoriesService.GetAll()
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

            if (ViewData["Categories"] == null)
            {
                throw new ArgumentException("Categories are empty");
            }

            var products = this.categoriesService.GetAllProductsFromCategory(id);
            var model = new AllProductsByCategoryViewModel
            {
                Products = products
            };

            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            await this.categoriesService.Create(name);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
