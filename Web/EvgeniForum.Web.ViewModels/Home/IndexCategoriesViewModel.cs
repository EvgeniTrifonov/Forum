namespace EvgeniForum.Web.ViewModels.Home
{
    public class IndexCategoriesViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Url => $"/{this.Name.Replace(' ', '-')}";
    }
}
