using DeveloperStore.Domain.Entities.Base;
using DeveloperStore.Repositories;
using DeveloperStore.Repositories.DataMapper;
using DeveloperStore.Repositories.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace DeveloperStore.Repositories;

public interface IExtendedContext<ContextType> where ContextType : DbContext
{
    ContextType Context { get; }
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<IPagedList<Projection>> GetPagedListAsync<Table, Projection>(int page = 1, int pageSize = 10, string order = "", Expression<Func<Table, bool>>? whereExpression = null) where Table : SimpleEntityBase;
    Task<Projection?> GetAsync<Table, Projection>(int? id = null) where Table : SimpleEntityBase;
    Task<int> CreateAsync<Table>(object data) where Table : SimpleEntityBase, new();
    Task UpdateAsync<Table>(int id, object data) where Table : SimpleEntityBase, new();
    Task<bool> DeleteAsync<Table>(int id) where Table : SimpleEntityBase, new();
}

public class ExtendedContext<ContextType> : IExtendedContext<ContextType> where ContextType : DbContext
{
    public ExtendedContext(ContextType context)
        => Context = context;

    public ContextType Context { get; }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await Context.Database.BeginTransactionAsync();

    #region GET

    public async Task<IPagedList<Projection>> GetPagedListAsync<Table, Projection>(
        int page = 1,
        int pageSize = 10,
        string order = "",
        Expression<Func<Table, bool>>? whereExpression = null
    ) where Table : SimpleEntityBase
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        var query = Context.Set<Table>().AsNoTracking();

        if (whereExpression != null)
        {
            query = query.Where(whereExpression);
        }

        var total = await query.CountAsync();

        if (!string.IsNullOrEmpty(order))
        {
            var orderByClauses = order.Split(',')
                .Select(o => o.Trim())
                .ToList();

            IOrderedQueryable<Table>? orderedQuery = null;

            foreach (var clause in orderByClauses)
            {
                if (!OrderByValidator.TryParseOrderBy(clause, typeof(Table), out var property, out var isDescending))
                {
                    throw new ArgumentException($"The sorting field '{clause}' is not valid.");
                }

                var parameter = Expression.Parameter(typeof(Table), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(propertyExpression, parameter);

                var orderMethod = orderedQuery == null
                    ? (isDescending ? "OrderByDescending" : "OrderBy")
                    : (isDescending ? "ThenByDescending" : "ThenBy");

                // Aplica a ordenação
                var resultExpression = Expression.Call(
                    typeof(Queryable),
                    orderMethod,
                    new Type[] { typeof(Table), propertyExpression.Type },
                    orderedQuery?.Expression ?? query.Expression,
                    lambda
                );

                orderedQuery = orderedQuery == null
                    ? (IOrderedQueryable<Table>)query.Provider.CreateQuery<Table>(resultExpression)
                    : (IOrderedQueryable<Table>)orderedQuery.Provider.CreateQuery<Table>(resultExpression);
            }

            query = orderedQuery ?? query;
        }

        var items = await query
            .ProjectToType<Projection>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Projection>(page, pageSize, total, items);
    }

    public async Task<Projection?> GetAsync<Table, Projection>(int? id = null)
        where Table : SimpleEntityBase
    {
        Expression<Func<Table, bool>> finalWhere;

        if (id.HasValue && id.Value > 0)
            finalWhere = i => i.Id == id.Value;
        else
            throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser maior que 0.");

        return await Context.Set<Table>()
            .AsNoTracking()
            .Where(finalWhere)
            .ProjectToType<Projection>()
            .FirstOrDefaultAsync();
    }

    #endregion

    #region POST

    public async Task<int> CreateAsync<Table>(object data) where Table : SimpleEntityBase, new()
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var row = SimpleMapper.Map<Table>(data);

        Context.Add(row);

        try
        {
            await Context.SaveChangesAsync();
        }
        finally
        {
            Context.Entry(row).State = EntityState.Detached;
        }

        return row.Id;
    }


    #endregion

    #region PUT

    public async Task UpdateAsync<Table>(
        int id,
        object data
    ) where Table : SimpleEntityBase, new()
    {
        ArgumentOutOfRangeException.ThrowIfZero(id);

        ArgumentNullException.ThrowIfNull(data);

        var target = new Table { Id = id };

        var properties = SimpleMapper.Map(data, target);

        Context.Attach(target);
        try
        {
            var entry = Context.Entry(target);

            foreach (var propertyName in properties)
                entry.Property(propertyName).IsModified = true;

            await Context.SaveChangesAsync();
        }
        finally
        {
            Context.Entry(target).State = EntityState.Detached;
        }
    }

    #endregion

    #region DELETE
    public async Task<bool> DeleteAsync<Table>(int id) where Table : SimpleEntityBase, new()
    {
        ArgumentOutOfRangeException.ThrowIfZero(id);

        var entity = await Context.Set<Table>()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (entity == null)
            return false; 

        Context.Set<Table>().Remove(entity);
        var affected = await Context.SaveChangesAsync();

        return affected > 0;
    }

    #endregion
}
