using Go_Saku.Net.Models;
using Microsoft.EntityFrameworkCore;

namespace Go_Saku.Net.Data
{
    public class UserApiDbContext : DbContext
    {
        public UserApiDbContext(DbContextOptions<UserApiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Badge> Badges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("mst_users");
            modelBuilder.Entity<Badge>().ToTable("mst_badges");
        }
    }
}
