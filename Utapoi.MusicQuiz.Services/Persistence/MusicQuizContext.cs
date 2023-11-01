using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Utapoi.MusicQuiz.Application.Persistence;
using Utapoi.MusicQuiz.Core.Entities;
using Utapoi.MusicQuiz.Infrastructure.Persistence.Interceptors;

namespace Utapoi.MusicQuiz.Infrastructure.Persistence;

public sealed class MusicQuizContext : DbContext, IMusicQuizContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MusicQuizContext" /> class.
    /// </summary>
    /// <param name="options">
    ///     The <see cref="DbContextOptions{TContext}" />.
    /// </param>
    /// <param name="auditableEntitySaveChangesInterceptor">
    ///     The <see cref="AuditableEntitySaveChangesInterceptor" />.
    /// </param>
    public MusicQuizContext(DbContextOptions<MusicQuizContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<User> Users => Set<User>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(MusicQuizContext)) ??
                                                     Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}