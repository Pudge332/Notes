using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;


namespace Notes.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<NoteDbContext>(options =>
            {
                options.UseSqlServer(@"Server=.\SQLEXPRESS;Database=NotesDB;Trusted_Connection=True;Trust Server Certificate=true;");
            });

            services.AddScoped<INotesDbContext>(provider => provider.GetService<NoteDbContext>());

            return services;
        }
    }
}
