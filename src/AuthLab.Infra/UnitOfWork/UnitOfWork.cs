using AuthLab.Application.UnitOfWork;
using AuthLab.Infra.DbContext;

namespace AuthLab.Infra.UnitOfWork;

public class UnitOfWork<T>(AuthLabDbContext context, IGenericRepository<T> repository) : IUnitOfWork<T> where T : class
{
    private readonly AuthLabDbContext _context = context;
    public IGenericRepository<T> Repository { get; set; } = repository;

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}