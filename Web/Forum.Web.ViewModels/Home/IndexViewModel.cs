namespace Forum.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexCategoriesViewModel> Categories { get; set; }
    }
}
