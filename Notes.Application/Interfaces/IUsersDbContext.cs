using Microsoft.EntityFrameworkCore;
using Notes.Users;

namespace Notes.Application.Interfaces
{
    public interface IUsersDbContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
