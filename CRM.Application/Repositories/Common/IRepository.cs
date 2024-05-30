using System.Linq.Expressions;

namespace CRM.Application.Repositories.Common;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? GetById(int id);
    Task<TEntity?> GetByIdAsync(int id);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    void Add(TEntity entity);
    bool Any(Expression<Func<TEntity, bool>> expression);
    void AddRange(IEnumerable<TEntity>? entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);


    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
}