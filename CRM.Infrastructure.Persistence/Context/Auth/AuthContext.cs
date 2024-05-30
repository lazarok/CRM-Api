using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Infrastructure.Persistence.Context.Auth;

public class AuthContext : DbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.RollbackTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var desiredNamespace = nameof(Configurations.Auth);
        var configurationTypes = typeof(AuthContext).Assembly.GetTypes()
            .Where(t => 
                t.Namespace != null && t.Namespace.StartsWith(desiredNamespace) 
                                    && t.IsClass 
                                    && !t.IsAbstract
                                    && t.GetInterfaces()
                                        .Any(i =>
                                            i.IsGenericType 
                                            && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            );

        foreach (var configurationType in configurationTypes)
        {
            dynamic configurationInstance = Activator.CreateInstance(configurationType)!;
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
        
        base.OnModelCreating(modelBuilder);
    }
}