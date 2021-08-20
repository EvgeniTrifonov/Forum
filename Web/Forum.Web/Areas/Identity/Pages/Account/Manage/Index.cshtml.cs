namespace Forum.Web.Areas.Identity.Pages.Account.Manage
{
    using System.IO;
    using System.Threading.Tasks;

    using Forum.Data;
    using Forum.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public UserViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            this.Input = new UserViewModel
            {
                Id = user.Id,
                UserImage = user.UserImage,
                NickName = user.NickName,
            };
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            var user = this.db.Users.Find(this.Input.Id);

            if (user == null)
            {
                return this.Page();
            }

            if (image != null)
            {
                var imageInMemory = new MemoryStream();
                image.CopyTo(imageInMemory);
                var imageBytes = imageInMemory.ToArray();
                user.UserImage = imageBytes;
            }

            user.NickName = this.Input.NickName;

            await this.db.SaveChangesAsync();

            return this.Page();
        }

        public class UserViewModel
        {
            [BindProperty]
            public string Id { get; set; }

            [BindProperty]
            public byte[] UserImage { get; set; }

            [BindProperty]
            public string NickName { get; set; }
        }
    }
}
