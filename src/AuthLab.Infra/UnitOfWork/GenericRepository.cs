using AuthLab.Application.UnitOfWork;
using AuthLab.Infra.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuthLab.Infra.UnitOfWork;

public class GenericRepository<T>(AuthLabDbContext context) : IGenericRepository<T> where T : class
{
    private readonly AuthLabDbContext _context = context;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?>? GetByIdAsync(int id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public T UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public async Task<T>? DeleteAsync(int id)   
    {
        var entity = await GetByIdAsync(id)!;
        if (entity == null) return null!;

        _context.Set<T>().Remove(entity);
        return entity;
    }
}