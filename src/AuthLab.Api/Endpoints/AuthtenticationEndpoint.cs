using AuthLab.Application.IJwtService;
using AuthLab.Application.UnitOfWork;
using AuthLab.Application.UnitOfWork.Requests;
using AuthLab.Application.UnitOfWork.Responses;
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

    static async Task<IResult> HandleRegister(User userRequest, IUnitOfWork<User> unitOfWork, IJwtService jwtService)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
        var user = new User(userRequest.Email, userRequest.Username, passwordHash);

        await unitOfWork.Repository().AddAsync(user);
        await unitOfWork.SaveChangesAsync();
        return Results.Created($"/auth-lab/api/users/{user.Id}", new UserResponse(user.Id, user.Email, user.Username));
    }

    static async Task<IResult> HandleLogin(LoginRequest loginRequest, IUnitOfWork<User> unitOfWork, IJwtService jwtService)
    {
        var user = await unitOfWork.Repository().GetByFuncAsync(x => x.Email == loginRequest.Email)! ?? throw new Exception("User not found");
        var jwtKey = jwtService.CreateToken(user);

        return BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password)
        ? Results.Ok(new { token = jwtKey })
        : Results.Unauthorized();
    }

    #endregion
}