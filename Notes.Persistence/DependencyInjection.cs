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
            var contionString = configuration["DbConnection"];
            services.AddDbContext<NoteDbContext>(options =>
            {
                options.UseSqlServer(contionString);
            });

            services.AddScoped<INotesDbContext>(provider => provider.GetService<NoteDbContext>());

            return services;
        }
    }
}
