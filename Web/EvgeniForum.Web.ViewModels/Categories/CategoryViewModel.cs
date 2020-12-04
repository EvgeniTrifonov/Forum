namespace EvgeniForum.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public IEnumerable<PostInCategoryViewModel> Posts { get; set; }
    }
}
