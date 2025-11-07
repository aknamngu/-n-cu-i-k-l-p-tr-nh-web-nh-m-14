namespace SweetAndSavoryBakery.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SweetAndSavoryBakery.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SweetAndSavoryBakery.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SweetAndSavoryBakery.Models.ApplicationDbContext context)
        {
            // 🔹 1. Seed Role và Admin (theo thầy)
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists(AppRoles.Admin))
                roleManager.Create(new IdentityRole(AppRoles.Admin));
            if (!roleManager.RoleExists(AppRoles.Customer))
                roleManager.Create(new IdentityRole(AppRoles.Customer));

            var adminEmail = "admin@gmail.com";
            var adminUser = userManager.FindByEmail(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = userManager.Create(adminUser, "Admin@123");
                if (!result.Succeeded)
                    throw new System.Exception(string.Join("; ", result.Errors));
            }

            if (!userManager.IsInRole(adminUser.Id, AppRoles.Admin))
                userManager.AddToRole(adminUser.Id, AppRoles.Admin);

            // 🔹 2. Seed dữ liệu mẫu cho bánh (theo bạn)
            context.Categories.AddOrUpdate(
                c => c.Name,
                new Category { Name = "Bánh ngọt", Description = "Ngọt dịu, mềm xốp" },
                new Category { Name = "Bánh mặn", Description = "Đậm đà, thơm béo" }
            );

            context.Products.AddOrUpdate(
                p => p.Name,
                new Product { Name = "Bánh su kem", Price = 25000, CategoryId = 1, ImageUrl = "/Content/images/sweet-cake.jpg" },
                new Product { Name = "Bánh mặn xúc xích", Price = 30000, CategoryId = 2, ImageUrl = "/Content/images/savory-bread.jpg" }
            );
        }

    }
}
