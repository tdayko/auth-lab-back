using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;

namespace AuthLab.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var userEndpoint = endpoints.MapGroup("auth-lab/api/users").WithTags("Users");

        userEndpoint.MapPost("", HandleAddUser)
            .Produces<User>(StatusCodes.Status201Created)
            .WithOpenApi(x =>
            {
                x.Summary = "Add a User";
                return x;
            });

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

        userEndpoint.MapPut("", HandleUpdateUser)
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(x =>
            {
                x.Summary = "Update a User";
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
            var result = await unitOfWork.Repository().GetByIdAsync(id)!;
            return result == null ? Results.NotFound(id) : Results.Ok(result);
        }

        static async Task<IResult> HandleUpdateUser(User user, IUnitOfWork<User> unitOfWork)
        {
            var result = unitOfWork.Repository().UpdateAsync(user);
            if (result == null) return Results.NotFound(user);

            await unitOfWork.SaveChangesAsync();
            return Results.Ok(user);
        }

        static async Task<IResult> HandleDeleteUser(int id, IUnitOfWork<User> unitOfWork)
        {
            var result = await unitOfWork.Repository().DeleteAsync(id)!;
            if (result == null) return Results.NotFound(new { Id = id });

            await unitOfWork.SaveChangesAsync();
            return Results.Ok(result);
        }

        #endregion

        return endpoints;
    }
}