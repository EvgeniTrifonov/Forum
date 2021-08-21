namespace Forum.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Forum.Common;
    using Forum.Data;
    using Forum.Data.Common.Repositories;
    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Post> postsRepo;
        private readonly ApplicationDbContext db;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db,
            IDeletableEntityRepository<Post> postsRepo)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.db = db;
            this.postsRepo = postsRepo;
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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.GetPosts().Include(p => p.Category).Include(p => p.User);
            return this.View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.GetPosts()
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = this.GetPosts().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewData["CategoryId"] = new SelectList(this.db.Categories, "Id", "Id", post.CategoryId);
            this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", post.UserId);
            return this.View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Content,UserId,CategoryId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Post post)
        {
            if (id != post.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.db.Update(post);
                    await this.db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.PostExists(post.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["CategoryId"] = new SelectList(this.db.Categories, "Id", "Id", post.CategoryId);
            this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", post.UserId);
            return this.View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.db.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await this.db.Posts.FindAsync(id);
            this.db.Posts.Remove(post);
            await this.db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private IQueryable<Post> GetPosts()
        {
            IQueryable<Post> result = null;
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                result = this.postsRepo.AllWithDeleted();
            }
            else
            {
                result = this.postsRepo.AllWithDeleted()
                    .Where(p => p.UserId == this.userManager.GetUserId(this.User));
            }

            return result;
        }

        private bool PostExists(int id)
        {
            return this.db.Posts.Any(e => e.Id == id);
        }
    }
}
