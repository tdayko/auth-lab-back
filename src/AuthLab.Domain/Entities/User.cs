namespace AuthLab.Domain.Entities;

public class User(string username, string pasword)
{
    public int Id { get; private set; }
    public string Username { get; private set; } = username;
    public string Password { get; private set; } = pasword;
}