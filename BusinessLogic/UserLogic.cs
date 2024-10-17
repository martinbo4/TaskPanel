using System.Text;
using BusinessLogic.DTOs;
using Domain;
using Memory;

namespace BusinessLogic;

public class UserLogic
{
    private readonly UserMemory _userMemory;

    public UserLogic(UserMemory userMemory)
    {
        _userMemory = userMemory;
    }

    public void CreateUser(UserDto userDto)
    {
        ValidateUserDto(userDto);
        UserExists(userDto.LogInDto!.Email!);
        User newUser = new User()
        {
            LogIn = userDto.LogInDto.ToLogIn(),
            FirstNameAndLastName = userDto.FirstNameAndLastName,
            Birthdate = userDto.Birthdate,
            IsAdmin = userDto.IsAdmin
        };
        _userMemory.AddUser(newUser);
    }
    
    public List<UserDto> GetAllUsers()
    {
        List<UserDto> userDtos = new List<UserDto>();
        foreach (User user in _userMemory.UserList)
        {
            userDtos.Add(UserDto.FromUser(user));
        }
        return userDtos;
    }
    
    public void ModifyUser(string oldEmail, UserDto dtoUser)
    {
        ValidateUserDto(dtoUser);
        User user = _userMemory.GetUserByEmail(oldEmail);
        user.LogIn = dtoUser.LogInDto!.ToLogIn();
        user.FirstNameAndLastName = dtoUser.FirstNameAndLastName;
        user.Birthdate = (DateTime)dtoUser.Birthdate!;
        user.IsAdmin = dtoUser.IsAdmin;
    }
    
    public void DeleteUserByEmail(string email)
    {
        User user = _userMemory.GetUserByEmail(email);
        _userMemory.DeleteUser(user);
    }
    
    public UserDto GetUserByEmail(string email)
    {
        User user = _userMemory.GetUserByEmail(email);
        return UserDto.FromUser(user);
    }
    
    public UserDto GetUserById(int id)
    {
        User user = _userMemory.GetUserById(id);
        return UserDto.FromUser(user);
    }

    private static void ValidateUserDto(UserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.FirstNameAndLastName))
        {
            throw new ArgumentException("Name and Surname are required");
        }
        if (userDto.Birthdate == null)
        {
            throw new ArgumentException("Birthdate is required");
        }
        if (userDto.LogInDto == null)
        {
            throw new ArgumentException("Email and Password are required");
        }
        if (string.IsNullOrWhiteSpace(userDto.LogInDto.Email))
        {
            throw new ArgumentException("Email is required");
        }
        if (string.IsNullOrWhiteSpace(userDto.LogInDto.Password))
        {
            throw new ArgumentException("Password is required");
        }
    }
    
    private void UserExists(string email)
    {
        if (_userMemory.UserList.Any(u => u.LogIn!.Email == email))
        {
            throw new ArgumentException("User already exists");
        }
    }
    
    public static string GenerateRandomPassword()
    {
        const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        const string digitChars = "0123456789";
        const string specialChars = "#@$.,%";
        const string allChars = upperChars + lowerChars + digitChars + specialChars;

        StringBuilder password = new StringBuilder();
        Random random = new Random();

        password.Append(upperChars[random.Next(upperChars.Length)]);
        password.Append(lowerChars[random.Next(lowerChars.Length)]);
        password.Append(digitChars[random.Next(digitChars.Length)]);
        password.Append(specialChars[random.Next(specialChars.Length)]);

        int remainingLength = random.Next(4, 8);
        for (int i = 0; i < remainingLength; i++)
        {
            password.Append(allChars[random.Next(allChars.Length)]);
        }
        return new string(password.ToString().ToCharArray().OrderBy(_ => random.Next()).ToArray());
    }
}