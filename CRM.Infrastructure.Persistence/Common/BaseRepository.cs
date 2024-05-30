using System.Linq.Expressions;
using CRM.Application.Repositories.Common;
using CRM.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Persistence.Common;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity>? entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
    
    public void UpdateRange(params TEntity[] entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Where(expression);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public TEntity? GetById(long id)
    {
        return _context.Set<TEntity>().Find(id);
    }
    
    public async Task<TEntity?> GetByIdAsync(long id,  string[]? eagerIncludes = null, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Set<TEntity>()
            .AsNoTracking();

        if (eagerIncludes?.Any() == true)
        {
            foreach (var eagerInclude in eagerIncludes)
            {
                baseQuery = baseQuery.Include(eagerInclude);
            }
        }
        
        cancellationToken.ThrowIfCancellationRequested();
        
        return await baseQuery.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Any(expression);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }
    
    
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        string[]? eagerIncludes = null,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Set<TEntity>()
            .AsNoTracking();

        if (eagerIncludes?.Any() == true)
        {
            foreach (var eagerInclude in eagerIncludes)
            {
                baseQuery = baseQuery.Include(eagerInclude);
            }
        }
        
        cancellationToken.ThrowIfCancellationRequested();

        return baseQuery.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }
    
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().CountAsync(predicate, cancellationToken);
    }
}