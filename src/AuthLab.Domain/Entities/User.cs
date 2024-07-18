namespace AuthLab.Domain.Entities;

public class User(string username, string password)
{
    public int? Id { get; }
    public string Username { get; private set; } = username;
    public string Password { get; private set; } = password;
}