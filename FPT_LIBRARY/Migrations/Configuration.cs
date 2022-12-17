namespace FPT_LIBRARY.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using FPT_LIBRARY.Models;
    using FPT_LIBRARY.Models.EF;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class DbMigrationsConfig : DbMigrationsConfiguration<FPT_LIBRARY.Models.ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "FPT_LIBRARY.Models.ApplicationDbContext";
        }

        protected override void Seed(FPT_LIBRARY.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminPassword = adminUserName;
                var adminPhoneNumber = "0385190202";
                var adminFirstName = "admin";
                var adminLastName = "user";
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminPhoneNumber, adminUserName, adminPassword, adminFirstName, adminLastName, adminRole);
            }
            if (!context.ProductCategories.Any())
            {
                CreateBookCategory(context, "Action", "/Uploads/images/genre/2.jpg");
                CreateBookCategory(context, "Classics", "/Uploads/images/genre/1.jpg");
                CreateBookCategory(context, "Fantasy", "/Uploads/images/genre/3.jpg");
                context.SaveChanges();
            }
            if (!context.Categories.Any())
            {
                CreateCategory(context, "Home", 1,true);
                CreateCategory(context, "Product", 2,true);
                CreateCategory(context, "Contact", 3, true);
                CreateCategory(context, "News", 100, false);
                context.SaveChanges();
            }
            if (!context.News.Any())
            {
                CreateNews(context,"Pham Van Hiep", "Is Mother Dead by Vigdis Hjorth – a daughter’s lament", true, "/Uploads/images/news/5248.jpg");
                CreateNews(context,"Le Huu Khuong","‘I’m parking my guilt’: confessions of a celebrity children’s book ghost writer", true, "/Uploads/images/news/6240.jpg");
                CreateNews(context, "Nuyen Phuc Anh", "Innocence, sex and war: Geoff Dyer on why the Go-Between is a novel for our time", true, "/Uploads/images/news/4284.jpg");
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                CreateBook(context, "The Heroes Of Olympus", 3, 5, 0);
                CreateBook(context, "Percy Jackson", 3, 10, 0);
                CreateBook(context, "Sherlock Holmes: The Complete Novels and Stories Volume II", 2, 9, 7);
                CreateBook(context, "Magnus Chase And The Gods Of Asgard Book 3: The Ship Of The Dead", 1, 20, 9);
                CreateBook(context, "After We Collided (The After Series Book 2)", 2, 17, 15);
                CreateBook(context, "After Ever Happy (The After Series Book 4)", 2, 19, 15);
                CreateBook(context, "Star Wars Vol. 13: Rogues And Rebels", 1, 29, 21);
                CreateBook(context, "The Kane Chronicles 1: The Red Pyramid", 3, 30, 25);
                CreateBook(context, "Star Wars: Darth Vader Vol. 2: Shadows And Secrets\r\n", 1, 28, 0);
                context.SaveChanges();
            }
            if (!context.ProductImages.Any())
            {
                ImageBook(context, 1, false, "/Uploads/images/books/1.png");
                ImageBook(context, 1, false, "/Uploads/images/books/2.png");
                ImageBook(context, 1, true, "/Uploads/images/books/3.jpg");
                ImageBook(context, 2, false, "/Uploads/images/books/4.jpg");
                ImageBook(context, 2, false, "/Uploads/images/books/5.jpg");
                ImageBook(context, 2, true, "/Uploads/images/books/6.jpg");
                ImageBook(context, 3, true, "/Uploads/images/books/7.jpg");
                ImageBook(context, 3, false, "/Uploads/images/books/8.jpg");
                ImageBook(context, 3, false, "/Uploads/images/books/9.png");
                ImageBook(context, 4, true, "/Uploads/images/books/10.jpg");
                ImageBook(context, 4, false, "/Uploads/images/books/11.jpg");
                ImageBook(context, 4, false, "/Uploads/images/books/12.png");
                ImageBook(context, 5, true, "/Uploads/images/books/13.jpg");
                ImageBook(context, 5, false, "/Uploads/images/books/14.png");
                ImageBook(context, 5, false, "/Uploads/images/books/15.png");
                ImageBook(context, 6, true, "/Uploads/images/books/16.jpg");
                ImageBook(context, 6, false, "/Uploads/images/books/17.jpg");
                ImageBook(context, 6, false, "/Uploads/images/books/18.jpg");
                ImageBook(context, 7, true, "/Uploads/images/books/19.jpg");
                ImageBook(context, 7, false, "/Uploads/images/books/20.jpg");
                ImageBook(context, 7, false, "/Uploads/images/books/21.jpg");
                ImageBook(context, 8, true, "/Uploads/images/books/22.jpg");
                ImageBook(context, 8, false, "/Uploads/images/books/23.jpg");
                ImageBook(context, 8, false, "/Uploads/images/books/24.jpg");
                ImageBook(context, 9, true, "/Uploads/images/books/25.jpg");
                ImageBook(context, 9, false, "/Uploads/images/books/26.jpg");
                ImageBook(context, 9, false, "/Uploads/images/books/27.png");
                context.SaveChanges();
            }

        }
        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminPhoneNumber, string adminUserName, string adminPassword, string adminFirstName, string adminLastName, string adminRole)
        {
            //Create the "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                PhoneNumber = adminPhoneNumber,
                FirstName = adminFirstName,
                LastName = adminLastName
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
            //Create the "Administrtator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            //Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }
        private void CreateBookCategory(ApplicationDbContext context,string title, string icon)
        {
            var cate = new ProductCategory();
            cate.Title = title;
            cate.Icon = icon;
            cate.CreateDate = DateTime.Now;
            cate.ModifiedDate= DateTime.Now;
            cate.Description = title;
            cate.SeoTitle = title;
            cate.Alias = FPT_LIBRARY.Models.Common.Filter.FilterChar(title);
            context.ProductCategories.Add(cate);
        }
        private void CreateCategory(ApplicationDbContext context, string title, int position, bool display)
        {
            var cate = new Category();
            cate.Title = title;
            cate.IsActive = display;
            cate.Position = position;
            cate.CreateDate = DateTime.Now;
            cate.ModifiedDate = DateTime.Now;
            cate.Description = title;
            cate.SeoTitle = title;
            cate.Alias = FPT_LIBRARY.Models.Common.Filter.FilterChar(title);
            context.Categories.Add(cate);
        }
        private void CreateNews(ApplicationDbContext context, string author , string title, bool display, string image)
        {
            var news = new News();
            news.CreateBy = author;
            news.Title = title;
            news.Image = image;
            news.IsActive = display;
            news.CategoryId = 4;
            news.CreateDate = DateTime.Now;
            news.ModifiedDate = DateTime.Now;
            news.Description = title;
            news.SeoTitle = title;
            news.Alias = FPT_LIBRARY.Models.Common.Filter.FilterChar(title);
            context.News.Add(news);
        }
        private void CreateBook(ApplicationDbContext context, string name, int cate, decimal price, decimal sale)
        {
            var book = new Product();
            book.Title = name;
            book.IsActive = true;
            book.IsHot = true;
            book.IsFeature= true;
            book.IsHome= true;
            book.IsSale= true;
            book.ProductCategoryId = cate;
            book.CreateDate = DateTime.Now;
            book.ModifiedDate = DateTime.Now;
            book.Description = name;
            book.SeoTitle = name;
            book.Price= price;
            book.PriceSale=sale;
            book.Alias = FPT_LIBRARY.Models.Common.Filter.FilterChar(name);
            context.Products.Add(book);
        }
        private void ImageBook(ApplicationDbContext context, int book, bool display, string image)
        {
            var imagebook = new ProductImage();
            imagebook.Image = image;
            imagebook.ProductId = book;
            imagebook.IsDefault = display;
            context.ProductImages.Add(imagebook);
        }


    }
}
