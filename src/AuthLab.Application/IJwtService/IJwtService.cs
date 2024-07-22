using AuthLab.Domain.Entities;

namespace AuthLab.Application.IJwtService;

public interface IJwtService
{
    public string CreateToken(User user);
}