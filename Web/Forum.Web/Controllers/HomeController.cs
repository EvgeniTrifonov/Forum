namespace Forum.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using Forum.Data;
    using Forum.Web.ViewModels;
    using Forum.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            var catrgorys = this.db.Categories.Select(c => new IndexCategoriesViewModel
            {
                Description = c.Description,
                ImgUrl = c.ImgUrl,
                Name = c.Name,
                Title = c.Title,
            }).ToList();
            viewModel.Categories = catrgorys;

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
