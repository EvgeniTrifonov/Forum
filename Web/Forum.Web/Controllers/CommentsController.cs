namespace Forum.Web.Controllers
{
    using System.Threading.Tasks;

    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentService commentService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.commentService.Create(input.PostId, userId, input.Content);
            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}
