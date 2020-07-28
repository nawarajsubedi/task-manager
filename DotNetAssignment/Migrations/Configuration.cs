using DotNetAssignment.Domain;
using System.Data.Entity.Migrations;

namespace DotNetAssignment.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name,
              new Microsoft.AspNet.Identity.EntityFramework.IdentityRole() { Name = "super_admin" },
              new Microsoft.AspNet.Identity.EntityFramework.IdentityRole() { Name = "admin" },
              new Microsoft.AspNet.Identity.EntityFramework.IdentityRole() { Name = "user" }
              );

        }
    }
}
