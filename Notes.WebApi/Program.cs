using System.Reflection;
using System.Text;
using Notes.WebApi.Authorization;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Application;
using Notes.Persistence;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = signingKey,
            ValidateIssuerSigningKey = true
        };
    });
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