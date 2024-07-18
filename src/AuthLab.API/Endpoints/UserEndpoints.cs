using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;

namespace AuthLab.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder addUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var userEndpoint = endpoints.MapGroup("auth-lab/api/users").WithTags("Users");

        userEndpoint.MapPost("", HandleAddUser);
        userEndpoint.MapGet("", HandleGetUsers);
        userEndpoint.MapGet("{id}", HandleGetUserById);
        userEndpoint.MapPut("{id}", HandleUpdateUser);
        userEndpoint.MapDelete("{id}", HandleDeleteUser);

        #region [privarte methods]

        static Task HandleAddUser(IUnitOfWork<User> unitOfWork)
        {
            return unitOfWork.Repository().GetAllAsync();
        }

        static Task HandleGetUsers(IUnitOfWork<User> unitOfWork)
        {
            return unitOfWork.Repository().GetAllAsync();
        }

        static Task? HandleGetUserById(int id, IUnitOfWork<User> unitOfWork)
        {
            return unitOfWork.Repository().GetByIdAsync(id);
        }

        static Task HandleUpdateUser(int id, User user, IUnitOfWork<User> unitOfWork)
        {
            return unitOfWork.Repository().UpdateAsync(user)!;
        }

        static Task HandleDeleteUser(int id, IUnitOfWork<User> unitOfWork)
        {
            return unitOfWork.Repository().DeleteAsync(id)!;
        }

        #endregion

        return endpoints;
    }
}