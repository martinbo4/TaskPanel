using Domain;

namespace BusinessLogic.DTOs;

public class LogInDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }

    public static LogInDto FromLogIn(LogIn logIn)
    {
        LogInDto dto = new LogInDto()
        {
            Email = logIn.Email,
            Password = logIn.Password
        };
        return dto;
    }

    public LogIn ToLogIn()
    {
        LogIn logIn = new LogIn(Email!, Password!);
        return logIn;
    }
}