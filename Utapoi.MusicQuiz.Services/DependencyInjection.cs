using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Application.Persistence;
using Utapoi.MusicQuiz.Application.Rooms;
using Utapoi.MusicQuiz.Application.Users;
using Utapoi.MusicQuiz.Infrastructure.Games;
using Utapoi.MusicQuiz.Infrastructure.Identity;
using Utapoi.MusicQuiz.Infrastructure.Persistence;
using Utapoi.MusicQuiz.Infrastructure.Persistence.Interceptors;
using Utapoi.MusicQuiz.Infrastructure.Rooms;
using Utapoi.MusicQuiz.Infrastructure.Users;

namespace Utapoi.MusicQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddSingleton<IMongoDatabase>(_ =>
            new MongoClient(configuration.GetConnectionString("MusicQuizDb")!).GetDatabase("MusicQuizDb"));

        // TODO: Remove once all services are updated.
        services.AddDbContext<MusicQuizContext>(x =>
        {
            x.UseMongoDB(configuration.GetConnectionString("MusicQuizDb")!, "MusicQuizDb");

            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors();
            x.LogTo(Console.WriteLine);
        });

        services.AddScoped<IMusicQuizContext>(provider => provider.GetRequiredService<MusicQuizContext>());

        services.AddSingleton<IGameManager, GameManager>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IRoomsService, RoomsService>();

        services.AddHttpClient("UtapoiHttpClient", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7244/"); // TODO: Load URL from appsettings.

            c.DefaultRequestHeaders.Add("Accept", "application/json");
            c.DefaultRequestHeaders.Add("User-Agent", "Utapoi.MusicQuiz.Server");
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<JwtBearerOptions, UtapoiJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, _ => {});

        services.AddAuthorization();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}