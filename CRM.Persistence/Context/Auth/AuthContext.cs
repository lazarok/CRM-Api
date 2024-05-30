using System.Reflection;
using CRM.Domain.Entities;
using CRM.Persistence.Configurations.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Persistence.Context.Auth;

public class AuthContext : DbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAuthConfigurations).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}