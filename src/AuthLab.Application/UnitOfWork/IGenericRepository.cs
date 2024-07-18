using System.Linq.Expressions;
namespace AuthLab.Application.UnitOfWork;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T?>?>? GetAllAsync();
    Task<T?>? GetByFuncAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}