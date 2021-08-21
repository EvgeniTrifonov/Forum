namespace Forum.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Forum.Data.Models;

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
                ("Sport", "https://webunwto.s3.eu-west-1.amazonaws.com/2020-01/sport-congresse.jpg"),
                ("News", "https://st.depositphotos.com/1011646/1255/i/950/depositphotos_12553000-stock-photo-breaking-news-screen.jpg"),
                ("Music", "http://researchvilladotcom.files.wordpress.com/2017/02/digitalmediawire-com.jpg"),
                ("Universe", "https://www.airlive.net/wp-content/uploads/2020/06/ad15bfd7b0_50164279_rotation-galaxie-spirale-univers.jpg"),
                ("AI", "https://images.idgesg.net/images/article/2019/11/ai_artificial_intelligence_ml_machine_learning_vector_by_kohb_gettyimages_1146634284-100817775-large.jpg"),
                ("Programing", "https://image.shutterstock.com/z/stock-vector--d-isometric-digital-design-programing-software-and-website-coding-man-on-the-computer-working-at-1307706505.jpg"),
                ("Physics", "https://media-cldnry.s-nbcnews.com/image/upload/t_fit-760w,f_auto,q_auto:best/newscms/2018_22/2451826/180601-atomi-mn-1540.jpg"),
                ("Math", "https://edukko.com/wp-content/uploads/2021/06/maths.jpeg"),
                ("Games", "https://image.shutterstock.com/image-vector/vector-illustration-neon-future-game-260nw-1861318969.jpg"),
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
