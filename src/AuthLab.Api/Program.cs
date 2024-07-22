using System.Text;

using AuthLab.Api.Endpoints;
using AuthLab.Infra;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);
ConfigureSwagger(builder);
ConfigureAuthentication(builder);

var app = builder.Build();

ConfigureMiddleware(app);
ConfigureEndpoints(app);

app.Run();

#region [configurations]

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthorization();
    builder.Services.AddInfra();
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(setup =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        setup.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
    });
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication()
        .AddJwtBearer(x =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SecretyKey").Value!);
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    );

    builder.Services.AddCors();
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    app.UseSwagger();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthLab v1"));
    app.UseHttpsRedirection();
}

void ConfigureEndpoints(WebApplication app)
{
    app.AddUserEndpoint();
    app.AddAuthenticationEndpoint();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

#endregion