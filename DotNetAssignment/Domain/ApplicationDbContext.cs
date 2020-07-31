using DotNetAssignment.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DotNetAssignment.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DatabaseConnectionString", throwIfV1Schema: false)
        {

        }

        public DbSet<TodoTask> Tasks { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}