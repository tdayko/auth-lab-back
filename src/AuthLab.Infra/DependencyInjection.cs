using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;
using AuthLab.Infra.DbContext;
using AuthLab.Infra.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;

namespace AuthLab.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork<User>, UnitOfWork<User>>();
        services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
        services.AddDbContext<AuthLabDbContext>();
        return services;
    }
}