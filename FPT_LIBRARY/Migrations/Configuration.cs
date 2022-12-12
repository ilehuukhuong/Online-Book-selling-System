namespace FPT_LIBRARY.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using FPT_LIBRARY.Models;
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
        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method
        //  to avoid creating duplicate seed data.
    }
}
