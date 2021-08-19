namespace Forum.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Forum.Data;
    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.db = db;
        }

        public IActionResult ById(int id)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);
            return this.View(postViewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            var viewModel = new PostCreateInputModel
            {
                Categories = categories,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var postId = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, user.Id);
            return this.RedirectToAction("ById", new { id = postId });
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserImage = user.UserImage,
                NickName = user.NickName,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(UserViewModel userView, IFormFile image)
        {
            if (image == null)
            {
                this.ModelState.AddModelError("Image", "Image is not valid!");
            }

            var imageInMemory = new MemoryStream();
            image.CopyTo(imageInMemory);
            var imageBytes = imageInMemory.ToArray();

            if (!this.ModelState.IsValid)
            {
                return this.View(userView);
            }

            var user = this.db.Users.Find(userView.Id);

            if (user == null)
            {
                throw new System.Exception("Error!");
            }

            user.UserImage = imageBytes;
            user.NickName = userView.NickName;

            await this.db.SaveChangesAsync();

            return this.RedirectToAction("Index", "Home");
        }
    }
}
