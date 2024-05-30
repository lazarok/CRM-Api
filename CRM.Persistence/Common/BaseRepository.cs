using System.Linq.Expressions;
using CRM.Application.Repositories.Common;
using CRM.Persistence.Context.Auth;
using Microsoft.EntityFrameworkCore;

namespace CRM.Persistence.Common;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AuthContext _context;

    public BaseRepository(AuthContext context)
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

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Where(expression);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public TEntity? GetById(int id)
    {
        return _context.Set<TEntity>().Find(id);
    }
    
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
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
    
    
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }
}