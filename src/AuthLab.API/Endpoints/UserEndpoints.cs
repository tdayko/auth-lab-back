using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;

namespace AuthLab.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var userEndpoint = endpoints.MapGroup("auth-lab/api/users").WithTags("Users");

        userEndpoint.MapPost("", HandleAddUser);
        userEndpoint.MapGet("", HandleGetUsers);
        userEndpoint.MapGet("{id}", HandleGetUserById);
        userEndpoint.MapPut("", HandleUpdateUser);
        userEndpoint.MapDelete("{id}", HandleDeleteUser);

        #region [privarte methods]

        static async Task<IResult> HandleAddUser(User user, IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            return Results.Created($"/auth-lab/api/users/{result.Id}", result);
        }

        static async Task<IResult> HandleGetUsers(IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().GetAllAsync()!;
            return result == null ? Results.NotFound() : Results.Ok(result);
        }

        static async Task<IResult> HandleGetUserById(int id, IUnitOfWork<User> unitOfWork)
        {
            var result =  await unitOfWork.Repository().GetByIdAsync(id)!;
            return result == null ? Results.NotFound(id) : Results.Ok(result);
        }

        static async Task<IResult> HandleUpdateUser(User user, IUnitOfWork<User> unitOfWork)
        {
            var result = unitOfWork.Repository().UpdateAsync(user);
            if(result == null) return Results.NotFound(user);

            await unitOfWork.SaveChangesAsync();
            return Results.Ok(user);
        }

        static async Task<IResult> HandleDeleteUser(int id, IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().DeleteAsync(id)!;
            if(result == null) return Results.NotFound(id);
            
            await unitOfWork.SaveChangesAsync();
            return Results.Ok(result);
        }

        #endregion

        return endpoints;
    }
}