using System.Text.RegularExpressions;

namespace Domain;

public class LogIn
{
    private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#@$.,%]).{8,}$";
    private const string RegexEmailPattern = @"^(?=.{1,100}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public string? Email { get; }
    public string? Password { get; }
    
    public LogIn(string? email, string password)
    {
        ValidateEmail(email);
        ValidatePassword(password);
        
        Email = email;
        Password = password;
    }
    
    public static void ValidatePassword(string password)
    {
        if (!Regex.IsMatch(password, PasswordPattern))
        {
            throw new ArgumentException("The password does not meet the requirements.");
        }
    }
    
    public static void ValidateEmail(string? email)
    {
        if (IsStringNull(email) || !Regex.IsMatch(email, RegexEmailPattern))
        {
            throw new ArgumentException("The email is not valid.");
        }
    }
    
    private static bool IsStringNull(string? text)
    {
        return string.IsNullOrEmpty(text);
    }
}
