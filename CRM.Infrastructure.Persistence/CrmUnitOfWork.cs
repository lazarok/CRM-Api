using CRM.Application.Repositories;
using CRM.Application.Repositories.Common;
using CRM.Domain.Common;
using CRM.Infrastructure.Persistence.Common;
using CRM.Infrastructure.Persistence.Context.Crm;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Infrastructure.Persistence;

public class CrmUnitOfWork : ICrmUnitOfWork
{
    private readonly CrmContext _context;
    private Dictionary<string,object> repositories;

    public CrmUnitOfWork(CrmContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        if (repositories == null)
        {
            repositories = new Dictionary<string,object>();
        }

        var type = typeof(T).Name;

        if (!repositories.TryGetValue(type, out object? value))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            value = repositoryInstance;
            repositories.Add(type, value);
        }
        return (BaseRepository<T>)value;
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
    
    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await _context.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default) =>
        await _context.CommitTransactionAsync(cancellationToken);

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default) =>
        await _context.RollbackTransactionAsync(cancellationToken);
    
    public void Dispose()
    {
        _context.Dispose();
    }
}