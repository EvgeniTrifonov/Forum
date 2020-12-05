namespace EvgeniForum.Web.ViewModels.Categories
{
    using EvgeniForum.Data.Models;
    using EvgeniForum.Services.Mapping;
    using System;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent =>
            this.Content?.Length > 300
            ? this.Content?.Substring(0, 300) + "..."
            : this.Content;

        public string UserUserName { get; set; }

        public int CommentsCount { get; set; }
    }
}
