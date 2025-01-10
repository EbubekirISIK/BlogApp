using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any()) // bir veya birden fazla migrations varsa uygula demektir.
                {
                    context.Database.Migrate(); //uygulama her çalıştırıldığında database sürekli sıfırdan oluşturulacak.
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Entity.Tag { Text = "Web Programlama", Url = "web-programlama", Color = Entity.TagsColor.warning },
                        new Entity.Tag { Text = "Backend", Url = "backend", Color = Entity.TagsColor.success },
                        new Entity.Tag { Text = "Frontend", Url = "frontend", Color = Entity.TagsColor.info },
                        new Entity.Tag { Text = "Fullstack", Url = "fullstack", Color = Entity.TagsColor.danger },
                        new Entity.Tag { Text = "Php", Url = "php", Color = Entity.TagsColor.secondary }
                        );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "sadikturan", Images = "avatar1.png", Email="info@sadikturan.com", Name="Sadık Turan", Password="123456" },
                        new Entity.User { UserName = "ahmetyilmaz", Images = "avatar2.png", Email = "info@ahmetyilmaz.com", Name = "Ahmet Yılmaz", Password = "123456" }
                        );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Entity.Post
                        {
                            Url = "aspnet-core",
                            Title = "Asp.net Core",
                            Content = "Asp.net core dersleri",
                            Description = "Asp.net core dersleri",
                            IsActive = true,
                            PublishesOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Images = "about.png",
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                new Comment {CommentText = "çok  iyi bir kurs. benim işime çok yaradı.", PublishedOn= DateTime.Now.AddDays(-5), UserId= 1},
                                new Comment {CommentText = "Beğendim.", PublishedOn=  DateTime.Now.AddDays(-5), UserId= 2},

                            }

                        },
                        new Entity.Post
                        {
                            Url = "php",

                            Title = "Php",
                            Content = "Php dersleri",
                            Description = "Php dersleri",
                            IsActive = true,
                            PublishesOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            Images = "saas-blog-post-1.jpg",
                            UserId = 2
                        },
                        new Entity.Post
                        {
                            Url = "Django-dersleri",

                            Title = "Django",
                            Content = "Django dersleri",
                            Description = "Django dersleri",
                            IsActive = true,
                            PublishesOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(4).ToList(),
                            Images = "WebTasarimSlide1.png",
                            UserId = 2
                        }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
