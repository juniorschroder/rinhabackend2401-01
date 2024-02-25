using System.Linq.Expressions;

namespace RinhaBackend.Domain.Abstractions.Repositories;

public interface IRepositoryBase<TEntity, TId> where TEntity : class
{
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filtros, int qtdeRegistros, 
        Expression<Func<TEntity, object>> orderBy);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TId id);
}