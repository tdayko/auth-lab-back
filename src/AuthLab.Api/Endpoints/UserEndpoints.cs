using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;

namespace AuthLab.Api.Endpoints;

internal static class UserEndpoints
{
    internal static IEndpointRouteBuilder AddUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var userEndpoint = endpoints.MapGroup("auth-lab/api/users").WithTags("Users");

        userEndpoint.MapGet("", HandleGetUsers)
            .Produces<IEnumerable<User>>(StatusCodes.Status200OK)
            .WithOpenApi(x =>
            {
                x.Summary = "Get all Users";
                return x;
            });

        userEndpoint.MapGet("{id}", HandleGetUserById)
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(x =>
            {
                x.Summary = "Get a User by Id";
                return x;
            });

        userEndpoint.MapDelete("{id}", HandleDeleteUser)
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(x =>
            {
                x.Summary = "Delete a User";
                return x;
            });

        #region [privarte methods]

        static async Task<IResult> HandleGetUsers(IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().GetAllAsync()!;
            return result == null ? Results.NotFound() : Results.Ok(result);
        }

        static async Task<IResult> HandleGetUserById(int id, IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().GetByFuncAsync(x => x.Id == id)!;
            return result == null ? Results.NotFound(id) : Results.Ok(result);
        }

        static async Task<IResult> HandleDeleteUser(int id, IUnitOfWork<User> unitOfWork)
        {
            var user = await unitOfWork.Repository().GetByFuncAsync(x => x.Id == id)!;
            if (user == null) return Results.NotFound(id);

            unitOfWork.Repository().DeleteAsync(user);
            await unitOfWork.SaveChangesAsync();
            return Results.Ok();
        }

        #endregion

        return endpoints;
    }
}