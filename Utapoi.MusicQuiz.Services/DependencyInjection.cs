using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utapoi.MusicQuiz.Application.Persistence;
using Utapoi.MusicQuiz.Application.Rooms;
using Utapoi.MusicQuiz.Application.Users;
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

        services.AddDbContext<MusicQuizContext>(x =>
        {
            x.UseMongoDB(configuration.GetConnectionString("MusicQuizDb")!, "MusicQuizDb");

            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors();
            x.LogTo(Console.WriteLine);
        });

        services.AddScoped<IMusicQuizContext>(provider => provider.GetRequiredService<MusicQuizContext>());

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IRoomsService, RoomsService>();

        return services;
    }
}