namespace Forum.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Forum.Data.Models;
    using Forum.Services.Mapping;
    using Ganss.XSS;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public byte[] UserImage { get; set; }

        public string NickName { get; set; }
    }
}
