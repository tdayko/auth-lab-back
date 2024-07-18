
using System.Linq.Expressions;

using AuthLab.Application.UnitOfWork;
using AuthLab.Infra.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuthLab.Infra.UnitOfWork;

public class GenericRepository<T>(AuthLabDbContext context) : IGenericRepository<T> where T : class
{
    private readonly AuthLabDbContext _context = context;

    public async Task<IEnumerable<T?>?>? GetAllAsync()
    {
        var result = await _context.Set<T>().ToListAsync();
        return result.Count == 0 ? null! : result;
    }

    public async Task<T?>? GetByFuncAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public void UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void DeleteAsync(T entity)   
    {
        _context.Set<T>().Remove(entity);
    }
}