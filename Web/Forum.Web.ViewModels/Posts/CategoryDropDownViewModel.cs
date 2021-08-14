namespace Forum.Web.ViewModels.Posts
{
    using Forum.Data.Models;
    using Forum.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
