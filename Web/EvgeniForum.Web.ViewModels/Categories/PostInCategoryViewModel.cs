namespace EvgeniForum.Web.ViewModels.Categories
{
    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Mapping;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string UserUserName { get; set; }

        public int CommentsCount { get; set; }
    }
}
