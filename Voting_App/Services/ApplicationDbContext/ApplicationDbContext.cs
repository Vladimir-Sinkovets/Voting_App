using Microsoft.EntityFrameworkCore;
using Voting_App.Models;

namespace Voting_App.Services.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set;  }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
