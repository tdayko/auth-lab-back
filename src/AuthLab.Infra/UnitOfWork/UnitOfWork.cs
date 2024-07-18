using AuthLab.Application.UnitOfWork;
using AuthLab.Infra.DbContext;

namespace AuthLab.Infra.UnitOfWork;

public class UnitOfWork<T>(AuthLabDbContext context, IGenericRepository<T> repository) : IUnitOfWork<T> where T : class
{
    private readonly AuthLabDbContext _context = context;
    private IGenericRepository<T> Repository { get; set; } = repository;

    IGenericRepository<T> IUnitOfWork<T>.Repository()
    {
        return Repository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}