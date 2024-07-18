using AuthLab.Api.Endpoints;
using AuthLab.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfra();

var app = builder.Build();

app.AddUserEndpoint();
app.AddAuthenticationEndpoint();

app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthLab v1"));
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.UseHttpsRedirection();
app.Run();
