using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Infrastructure.Persistence.Context.Crm;

public class CrmContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CrmContext(DbContextOptions<CrmContext> options)
        : base(options)
    {
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
        var configurationTypes = typeof(CrmContext).Assembly.GetTypes()
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