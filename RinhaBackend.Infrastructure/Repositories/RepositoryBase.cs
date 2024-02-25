using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Infrastructure.Data;

namespace RinhaBackend.Infrastructure.Repositories;

public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> where TEntity : class
{
    protected RinhaContext Context;
    internal DbSet<TEntity> dbSet;

    public RepositoryBase(RinhaContext context)
    {
        Context = context;
        dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filtros, int qtdeRegistros, Expression<Func<TEntity, object>> orderBy) =>
        await dbSet.AsNoTracking().Where(filtros).Take(qtdeRegistros).OrderByDescending(orderBy).ToListAsync();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var data = await dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return data.Entity;
    }

    public async Task<TEntity?> GetByIdAsync(TId id) => await dbSet.FindAsync(id);
}