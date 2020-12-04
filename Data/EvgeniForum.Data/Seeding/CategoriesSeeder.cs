namespace EvgeniForum.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EvgeniForum.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string ImgUrl)>
            {
                ("Sport", "https://media.npr.org/assets/img/2020/06/10/gettyimages-200199027-001-b5fb3d8d8469ab744d9e97706fa67bc5c0e4fa40.jpg"),
                ("New", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhpiNON39UBXjwmFR3gLH78i-oQe9DWpDkSw&usqp=CAU"),
                ("Music", "https://www.online-tech-tips.com/wp-content/uploads/2019/05/music.png"),
                ("Universe", "https://www.airlive.net/wp-content/uploads/2020/06/ad15bfd7b0_50164279_rotation-galaxie-spirale-univers.jpg"),
                ("AI", "https://images.idgesg.net/images/article/2019/11/ai_artificial_intelligence_ml_machine_learning_vector_by_kohb_gettyimages_1146634284-100817775-large.jpg"),
                ("Programing", "https://image.shutterstock.com/z/stock-vector--d-isometric-digital-design-programing-software-and-website-coding-man-on-the-computer-working-at-1307706505.jpg"),
                ("Physics", "https://www.thegreatcourses.com/media/catalog/product/cache/1/plus_image/800x451/0f396e8a55728e79b48334e699243c07/1/2/1260.1551365488.jpg"),
            };

            foreach (var (name, imgUrl) in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = name,
                    Description = name,
                    Title = name,
                    ImgUrl = imgUrl,
                });
            }
        }
    }
}
