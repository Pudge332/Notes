using AutoMapper;
using System.Reflection;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Application;
using Notes.Persistence;
using Notes.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();
Configure(app);
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
    });
    services.AddApplication();
    services.AddPersistence(configuration);
    services.AddControllers();
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin(); //В реальном приложении так не делать!
                                     // Должны присутсвовать ограничения
        });
    });

}
void Configure(WebApplication app) 
{
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllers();
    //});
    app.MapControllers();
    app.UseEndpoints(_ => { });

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception)
        {
        }
    }

}