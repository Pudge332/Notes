using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Persistence.EntityTypeConfiguration;
using Notes.Users;

namespace Notes.Persistence
{
    public sealed class UsersDbContext : DbContext, IUsersDbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
