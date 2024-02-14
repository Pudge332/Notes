using System.Reflection;
using System.Text;
using Notes.WebApi.Authorization;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Application;
using Notes.Persistence;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.WebApi.Controllers;
using Notes.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();
Configure(app);
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
    var secretKey = configuration.GetSection("JWTSettings:SecretKey").Value;
    var issuer = configuration.GetSection("JWTSettings.Issuer").Value;
    var audience = configuration.GetSection("JwtSetting.Audience").Value;
    var expiresHours = configuration.GetSection("JwtSetting.ExpiresHours").Value;
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["one-small-detail"];

                    return Task.CompletedTask;
                }
            };
        });
    services.AddAuthorization();
    services.AddControllersWithViews();
    services.AddSingleton<CurrentUserService, CurrentUserService>();
    services.AddScoped<JwtProvider, JwtProvider>();
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
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowAll");
    app.MapControllers();
    app.UseEndpoints(_ => { });

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
            DbInitializer.Initialize(context);
            var userContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            DbInitializer.Initialize(userContext);
        }
        catch (Exception)
        {
        }
    }

}