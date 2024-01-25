using Microsoft.EntityFrameworkCore;

namespace Notes.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(DbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
