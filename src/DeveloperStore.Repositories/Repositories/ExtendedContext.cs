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

    /// <summary>
    /// Obtém uma lista paginada de registros a partir de uma tabela
    /// </summary>
    /// <typeparam name="Table">A tabela de origem</typeparam>
    /// <typeparam name="Projection">O tipo de dados a ser retornado para cada registro</typeparam>
    /// <param name="whereExpression">Expressão opcional para filtrar os registros</param>
    /// <param name="order">Expressão de ordenação</param>
    /// <param name="page">Número da página para paginação</param>
    /// <param name="pageSize">Quantidade de registros por página</param>
    /// <returns>Uma página da lista de registros</returns>
    public async Task<IPagedList<Projection>> GetPagedListAsync<Table, Projection>(
        int page = 1,
        int pageSize = 10,
        string order = "",
        Expression<Func<Table, bool>>? whereExpression = null
    ) where Table : SimpleEntityBase
    {
        if (page <= 0)
            throw new ArgumentOutOfRangeException(nameof(page));

        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

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
                var descending = clause.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                var propertyName = descending ? clause.Substring(0, clause.Length - 5).Trim() : clause;

                if (!OrderByValidator.TryParseOrderBy(propertyName, typeof(Table), out var property, out var isDescending))
                {
                    throw new ArgumentException($"O campo de ordenação '{propertyName}' não é válido.");
                }

                var parameter = Expression.Parameter(typeof(Table), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(propertyExpression, parameter);

                var orderMethod = orderedQuery == null
                    ? (isDescending ? "OrderByDescending" : "OrderBy")
                    : (isDescending ? "ThenByDescending" : "ThenBy");

                var resultExpression = Expression.Call(
                    typeof(Queryable),
                    orderMethod,
                    new Type[] { typeof(Table), propertyExpression.Type },
                    orderedQuery?.Expression ?? query.Expression,
                    lambda
                );

                orderedQuery = orderedQuery == null
                    ? query.Provider.CreateQuery<Table>(resultExpression) as IOrderedQueryable<Table>
                    : orderedQuery.Provider.CreateQuery<Table>(resultExpression) as IOrderedQueryable<Table>;
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

    /// <summary>
    /// Retorna os dados de um registro de uma tabela a partir de seu ID ou a partir de uma expressão de filtro.
    /// </summary>
    /// <typeparam name="Table">A tabela de origem</typeparam>
    /// <typeparam name="Projection">O tipo do objeto a ser retornado</typeparam>
    /// <param name="id">O Id do registro. Pode ser nulo.</param>
    /// <param name="where">O filtro para o registro. Pode ser nulo.</param>
    /// <returns>
    /// Retorna um objeto com os dados do registro atual (todas as propriedades do objeto cujo nome coincidir
    /// com nomes de campo da tabela receberão os dados do registro) ou null caso o registro não seja encontrado.
    /// </returns>
    /// <exception cref="ArgumentException">Caso ambos os parâmetros sejam nulos ou inválidos.</exception>
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

    /// <summary>
    /// Efetua a inclusão de um registro
    /// </summary>
    /// <typeparam name="Table">A tabela de destino</typeparam>
    /// <param name="data">Os dados a serem incluídos no novo registro</param>
    /// <returns>O ID do registro inserido</returns>
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
    /// <summary>
    /// Efetua a alteração de um registro
    /// </summary>
    /// <typeparam name="Table">A tabela de destino</typeparam>
    /// <param name="id">O Id do registro a ser alterado</param>
    /// <param name="data">Os dados a serem gravados para o registro</param>
    /// <param name="auditInfo">Informações de auditoria</param>
    /// <returns></returns>
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
    ///// <summary>
    ///// Muda o valor do campo IsDeleted de um registro
    ///// </summary>
    ///// <typeparam name="Table">A tabela de destino</typeparam>
    ///// <param name="context">O contexto do banco de dados</param>
    ///// <param name="id">O ID do registro a ser atualizado</param>
    ///// <returns>
    ///// Valor lógico indicando se o registro foi modificado 
    ///// (retorna falso caso o id não exista ou IsDeleted já tenha o valor informado)
    ///// </returns>
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
