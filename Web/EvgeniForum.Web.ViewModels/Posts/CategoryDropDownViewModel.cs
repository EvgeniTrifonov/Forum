namespace EvgeniForum.Web.ViewModels.Posts
{
    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}