namespace EvgeniForum.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using EvgeniForum.Data.Common.Repositories;
    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Data;
    using EvgeniForum.Services.Mapping;
    using EvgeniForum.Web.ViewModels;
    using EvgeniForum.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Categories = this.categoriesService
                .GetAll<IndexCategoriesViewModel>(),
            };
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
