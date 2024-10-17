using Domain;

namespace BusinessLogic.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public LogInDto? LogInDto { get; set; }
    public string? FirstNameAndLastName { get; set; }
    public DateTime? Birthdate { get; set; }
    public bool IsAdmin { get; set; }
    
    public static UserDto FromUser(User user)
    {
        UserDto userDto = new UserDto()
        {
            Id = user.Id,
            LogInDto = LogInDto.FromLogIn(user.LogIn!),
            FirstNameAndLastName = user.FirstNameAndLastName,
            Birthdate = user.Birthdate,
            IsAdmin = user.IsAdmin
        };
        return userDto;
    }
    
    public User ToUser()
    {
        User user = new User()
        {
            Id = Id,
            Birthdate = Birthdate,
            FirstNameAndLastName = FirstNameAndLastName,
            IsAdmin = IsAdmin,
            LogIn = LogInDto!.ToLogIn()
        };
        
        return user;
    }
}