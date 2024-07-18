namespace AuthLab.Application.UnitOfWork.Requests;

public record UserRequest(string Email, string Username, string Password);