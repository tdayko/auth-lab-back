namespace AuthLab.Application.UnitOfWork;

public interface IUnitOfWork<T> where T : class
{
    IGenericRepository<T> Repository();
    Task<int> SaveChangesAsync();
    void Dispose();
}