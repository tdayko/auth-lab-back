using AuthLab.Infra.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace AuthLab.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddDbContext<AuthLabDbContext>();
        return services;
    }
}