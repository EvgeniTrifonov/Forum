namespace Forum.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Forum.Common;
    using Forum.Data;
    using Forum.Data.Common.Repositories;
    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Comment> commentRepo;
        private readonly ApplicationDbContext db;

        public CommentsController(
            ICommentService commentService,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Comment> commentRepo,
            ApplicationDbContext db)
        {
            this.commentService = commentService;
            this.userManager = userManager;
            this.commentRepo = commentRepo;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.GetComments().Include(c => c.Parent).Include(c => c.Post).Include(c => c.User);
            return this.View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var comment = await this.GetComments()
                .Include(c => c.Parent)
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return this.NotFound();
            }

            return this.View(comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var parentId =
                input.ParentId == 0 ?
                    (int?)null :
                    input.ParentId;

            if (parentId.HasValue)
            {
                if (!this.commentService.IsInPostId(parentId.Value, input.PostId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.commentService.Create(input.PostId, userId, input.Content, parentId);
            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var comment = this.GetComments().FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return this.NotFound();
            }

            this.ViewData["ParentId"] = new SelectList(this.db.Comments, "Id", "Id", comment.ParentId);
            this.ViewData["PostId"] = new SelectList(this.db.Posts, "Id", "UserId", comment.PostId);
            this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", comment.UserId);
            return this.View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,ParentId,Content,UserId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Comment comment)
        {
            if (id != comment.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.db.Update(comment);
                    await this.db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CommentExists(comment.Id))
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

            this.ViewData["ParentId"] = new SelectList(this.db.Comments, "Id", "Id", comment.ParentId);
            this.ViewData["PostId"] = new SelectList(this.db.Posts, "Id", "UserId", comment.PostId);
            this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", comment.UserId);
            return this.View(comment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var comment = await this.db.Comments
                .Include(c => c.Parent)
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return this.NotFound();
            }

            return this.View(comment);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await this.db.Comments.FindAsync(id);
            this.db.Comments.Remove(comment);
            await this.db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private IQueryable<Comment> GetComments()
        {
            IQueryable<Comment> result = null;
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                result = this.commentRepo.AllWithDeleted();
            }
            else
            {
                result = this.commentRepo.AllWithDeleted()
                    .Where(c => c.UserId == this.userManager.GetUserId(this.User));
            }

            return result;
        }

        private bool CommentExists(int id)
        {
            return this.db.Comments.Any(e => e.Id == id);
        }
    }
}
