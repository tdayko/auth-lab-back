namespace AuthLab.Application.UnitOfWork;

public interface IUnitOfWork<T> where T : class
{
    Task<int> SaveChangesAsync();
    void Dispose();
}