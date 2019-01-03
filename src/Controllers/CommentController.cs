using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentsService commentsService;

        private readonly UserManager<IdentityUser> _userManager;

        public CommentController(ICommentsService commentsService, UserManager<IdentityUser> userManager)
        {
            this.commentsService = commentsService;
            _userManager = userManager;
        }

        public IActionResult Add(string id)
        {
           var model = new CreateCommentInputModel
            {
                ProductId = id,
                UserId = _userManager.GetUserId(User)
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCommentInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var id = await this.commentsService.Add(model);

            return this.RedirectToAction("Details", new { id = id});
        }

        public IActionResult Details(string id)
        {
            var model = commentsService.GetById(id);

            return View(model);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var model = commentsService.GetById(id);
            if (!ModelState.IsValid || !model.User.UserName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CommentViewModel model)
        {
            var id = await commentsService.Edit(model);

            return RedirectToAction("Details", new {id = id});
        }

        
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var model = commentsService.GetById(id);
            if (!ModelState.IsValid || !model.User.UserName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await commentsService.Delete(id);

            return RedirectToAction("Deleted");
        }

        public IActionResult Deleted()
        {
            return View();
        }

        public IActionResult All(string id)
        {
            var model = this.commentsService.GetAllByProduct(id);

            return View(model);
        }
    }
}
