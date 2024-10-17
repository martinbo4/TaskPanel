namespace Domain;

public class User
{
    private static int _idCounter;
    public int Id { get; init; }
    public LogIn? LogIn { get; set; }
    public string? FirstNameAndLastName { get; set; }
    private DateTime? _birthdate;
    public DateTime? Birthdate
    {
        get => _birthdate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new ArgumentException("The date of birth cannot be in the future.");
            }
            _birthdate = value;
        }
    }
    public bool IsAdmin { get; set; }

    public User()
    {
        _idCounter++;
        this.Id = _idCounter;
        this.LogIn = null;
        this.FirstNameAndLastName = null;
        this.Birthdate = null;
        this.IsAdmin = false;
    }
    public User(LogIn logIn, string firstNameAndLastName, DateTime birthdate, bool isAdmin)
    {
        ValidateFirstNameAndLastName(firstNameAndLastName);
        ValidateBirthDate(birthdate);
        _idCounter++;
        this.Id = _idCounter;
        this.LogIn = logIn;
        this.FirstNameAndLastName = firstNameAndLastName;
        this.Birthdate = birthdate;
        this.IsAdmin = isAdmin;
    }

    public static void ValidateFirstNameAndLastName(string firstNameAndLastName)
    {
        if (IsStringNull(firstNameAndLastName) || firstNameAndLastName.Length > 100)
        {
            throw new ArgumentException("The first and last name must not be empty or contain more than 100 characters.");
        }
    }

    public static void ValidateBirthDate(DateTime birthdate)
    {
        if (birthdate > DateTime.Now)
        {
            throw new ArgumentException("The date of birth cannot be in the future.");
        }
    }

    private static bool IsStringNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
