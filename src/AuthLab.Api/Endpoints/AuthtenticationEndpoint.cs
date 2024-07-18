using AuthLab.Application.UnitOfWork;
using AuthLab.Domain.Entities;

namespace AuthLab.Api.Endpoints;

internal static class AuthenticationEndpoint
{
    internal static IEndpointRouteBuilder AddAuthenticationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var authenticationEndpoint = endpoints.MapGroup("auth-lab/api/authentication").WithTags("Authentication");

        authenticationEndpoint.MapPost("register", HandleRegister)
            .Produces<User>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(x => 
            {
                x.Summary = "Register";
                return x;
            });

        authenticationEndpoint.MapPost("login", HandleLogin)
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(x =>
            {
                x.Summary = "Login";
                return x;
            });

        return endpoints;
    }

    #region [privarte methods]

    static async Task<IResult> HandleRegister(User user, IUnitOfWork<User> unitOfWork)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var result = new User(user.Email, user.Username, passwordHash);

        await unitOfWork.Repository().AddAsync(result);
        await unitOfWork.SaveChangesAsync();
        return Results.Created($"/auth-lab/api/users/{result.Id}", result);
    }

    static async Task<IResult> HandleLogin(User user, IUnitOfWork<User> unitOfWork)
    {
        var result = await unitOfWork.Repository().GetByFuncAsync(x => x.Email == user.Email)! ?? throw new Exception("User not found");
        return BCrypt.Net.BCrypt.Verify(user.Password, result.Password) ? Results.Ok() : Results.Unauthorized();
    }

    #endregion
}