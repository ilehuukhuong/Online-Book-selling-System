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
                CreateBookCategory(context, "Action and Adventure", "/Uploads/images/genre/2.jpg");
                CreateBookCategory(context, "Classics", "/Uploads/images/genre/1.jpg");
                CreateBookCategory(context, "Fantasy", "/Uploads/images/genre/3.jpg");
                context.SaveChanges();
            }
            if (!context.Categories.Any())
            {
                CreateCategory(context, "Home", 1);
                CreateCategory(context, "Product", 2);
                CreateCategory(context, "Contact", 3);
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

        private void CreateCategory(ApplicationDbContext context, string title, int position)
        {
            var cate = new Category();
            cate.Title = title;
            cate.IsActive = true;
            cate.Position = position;
            cate.CreateDate = DateTime.Now;
            cate.ModifiedDate = DateTime.Now;
            cate.Description = title;
            cate.SeoTitle = title;
            cate.Alias = FPT_LIBRARY.Models.Common.Filter.FilterChar(title);
            context.Categories.Add(cate);
        }

        //private void CreateBook(ApplicationDbContext db,
        //    string title, string body, DateTime date, string authorUsername)
        //{
        //    var book = new Product();
        //    book.Title = title;
        //    book.Body = body;
        //    book.Date = date;
        //    book.Author = context.Users.Where(u => u.UserName == authorUsername).FirstOrDefault();
        //    context.Posts.Add(post);
        //}

        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method
        //  to avoid creating duplicate seed data.
    }
}
