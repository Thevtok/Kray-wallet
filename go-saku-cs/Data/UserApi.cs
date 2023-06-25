using Go_Saku.Net.Models;
using go_saku_cs.Models;
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
        public DbSet<Bank> Banks { get; set; }
        public DbSet<PhotoUser> PhotoUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("mst_users");
            modelBuilder.Entity<Badge>().ToTable("mst_badges");
            modelBuilder.Entity<Bank>().ToTable("mst_bank_account");
            modelBuilder.Entity<PhotoUser>().ToTable("mst_photo_url");
        }
    }
}
