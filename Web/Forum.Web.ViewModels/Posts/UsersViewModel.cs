namespace Forum.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class UsersViewModel
    {
        public UsersViewModel()
        {
            this.Users = new HashSet<UserViewModel>();
        }

        public virtual ICollection<UserViewModel> Users { get; set; }
    }
}
