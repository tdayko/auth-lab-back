using AuthLab.Application.UnitOfWork;
using AuthLab.Application.UnitOfWork.Requests;
using AuthLab.Application.UnitOfWork.Responses;
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
            return result == null ? Results.NotFound() : Results.Ok(result.Select(x => new UserResponse(x!.Id, x.Email, x.Username)));
        }

        static async Task<IResult> HandleGetUserById(int id, IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().GetByFuncAsync(x => x.Id == id)!;
            return result == null ? Results.NotFound(new { id }) : Results.Ok(new UserResponse(result.Id, result.Email, result.Username));
        }

        static async Task<IResult> HandleDeleteUser(int id, IUnitOfWork<User> unitOfWork)
        {
            var user = await unitOfWork.Repository().GetByFuncAsync(x => x.Id == id)!;
            if (user == null) return Results.NotFound(new { id });

            unitOfWork.Repository().DeleteAsync(user);
            await unitOfWork.SaveChangesAsync();
            return Results.Ok();
        }

        #endregion

        return endpoints;
    }
}