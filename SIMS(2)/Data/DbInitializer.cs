using Microsoft.EntityFrameworkCore;
using SIMS_2_.Models;

namespace SIMS_2_.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            //context.Database.Migrate(); // Ensure the database is up to date

            //// Seed Roles if they don't exist
            //if (!context.Roles.Any())
            //{
            //    context.Roles.AddRange(
            //        new Role { RoleId = 1, RoleName = "Student" },
            //        new Role { RoleId = 2, RoleName = "Faculty" },
            //        new Role { RoleId = 3, RoleName = "Admin" }
            //    );
            //    context.SaveChanges();
            //}

            //// Seed an initial Admin user if none exists
            //if (!context.Users.Any(u => u.RoleId == 3))
            //{
            //    var adminUser = new User
            //    {
            //        UserName = "admin",
            //        Password = "admin123", // In a real app, hash the password
            //        Email = "admin@example.com",
            //        RoleId = 3,
            //        CreatedAt = DateTime.Now
            //    };
            //    context.Users.Add(adminUser);
            //    context.SaveChanges();

            //    var admin = new Admin
            //    {
            //        Name = "Admin User",
            //        UserId = adminUser.UserId
            //    };
            //    context.Admins.Add(admin);
            //    context.SaveChanges();
            //}
        }
    }
}