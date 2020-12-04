namespace EvgeniForum.Web.ViewModels.Home
{
    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Mapping;

    public class IndexCategoriesViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public int PostsCount { get; set; }

        public string Url => $"/{this.Name.Replace(' ', '-')}";
    }
}
