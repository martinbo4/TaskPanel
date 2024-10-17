using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using Memory;

namespace BusinnessLogic.Test;

[TestClass]
public class UserLogicTest
{
    private UserLogic _userLogic;
    private static LogIn _logIn1 = new LogIn("mateo@gmail.com", "Mateo123.");
    private static LogIn _logIn2 = new LogIn("mateopb@gmail.com", "Mateo123346.");
    private static DateTime _birthDate = new DateTime(2004, 03, 10);
    private static string _nameAndLastName = "Jane Doe";
    private static bool _isAdmin = false;
    private User _user1 = new User(_logIn1, _nameAndLastName, _birthDate, _isAdmin);
    private User _user2 = new User(_logIn2, _nameAndLastName, _birthDate, _isAdmin);
    private UserMemory _userMemory;

    [TestInitialize]
    public void Setup()
    {
        _userMemory = new UserMemory();
        _userLogic = new UserLogic(_userMemory);
        _userMemory.AddUser(_user1);
        _userMemory.AddUser(_user2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowException_When_UserAlreadyExist()
    {
        UserDto user = new UserDto()
        {
            LogInDto = LogInDto.FromLogIn(_logIn1),
            FirstNameAndLastName = _nameAndLastName,
            Birthdate = _birthDate,
            IsAdmin = _isAdmin
        };
        UserLogic userLogic = new UserLogic(_userMemory);
        
        userLogic.CreateUser(user);
    }
    
    [TestMethod]
    public void DeleteOneUser()
    {
        UserLogic userLogic = new UserLogic(_userMemory);

        userLogic.DeleteUserByEmail(_logIn1.Email);
        
        List<UserDto>? users = userLogic.GetAllUsers();
        Assert.IsFalse(users.Any(u => u.LogInDto.Email == _logIn1.Email));
        Assert.IsTrue(users.Any(u => u.LogInDto.Email == _logIn2.Email));
    }
    
    [TestMethod]
    public void GetAllUsersTest()
    {
        UserLogic userLogic = new UserLogic(_userMemory);

        List<UserDto>? users = userLogic.GetAllUsers();
        
        Assert.IsTrue(users.Any(u => u.LogInDto.Email == _logIn1.Email));
        Assert.IsTrue(users.Any(u => u.LogInDto.Email == _logIn2.Email));
    }
    
    [TestMethod]
    public void ModifyOneUser()
    {
        _userMemory = new UserMemory();
        _userMemory.AddUser(_user1);
        _userMemory.AddUser(_user2);
        UserLogic userLogic = new UserLogic(_userMemory);
        
        string? newEmail = "example3@email.com";
        string newPassword = "Mateo123.";
        string newNameAndLastName = "Jhon Hansen";
        DateTime newBirthDate = new DateTime(2000, 03, 10);
        bool newIsAdmin = true;
        
        UserDto user = new UserDto()
        {
            LogInDto = LogInDto.FromLogIn(new LogIn(newEmail, newPassword)),
            FirstNameAndLastName = newNameAndLastName,
            Birthdate = newBirthDate,
            IsAdmin = newIsAdmin
        };
        
        userLogic.ModifyUser(_logIn1.Email, user);
        
        List<UserDto>? users = userLogic.GetAllUsers();
        Assert.IsTrue(users.Any(u => u.LogInDto.Email == newEmail));
        Assert.IsTrue(users.Any(u => u.LogInDto.Password == newPassword));
        Assert.IsTrue(users.Any(u => u.FirstNameAndLastName == newNameAndLastName));
        Assert.IsTrue(users.Any(u => u.Birthdate == newBirthDate));
        Assert.IsTrue(users.Any(u => u.IsAdmin == newIsAdmin));
    }
    
    [TestMethod]
    public void GenerateRandomPasswordTest()
    {
        string password = UserLogic.GenerateRandomPassword();

        Assert.IsTrue(password.Length >= 8 && password.Length <= 12);
    }
    
    [TestMethod]
    public void GetUserByEmailReturnsUserWhenUserExists()
    {
        UserLogic userLogic = new UserLogic(_userMemory);
        var userDto = userLogic.GetUserByEmail(_logIn1.Email);
        var result = userDto.ToUser();
        Assert.AreEqual(_user1.LogIn.Email, result.LogIn.Email);
        Assert.AreEqual(_user1.LogIn.Password, result.LogIn.Password);
        Assert.AreEqual(_user1.FirstNameAndLastName, result.FirstNameAndLastName);
        Assert.AreEqual(_user1.Birthdate, result.Birthdate);
        Assert.AreEqual(_user1.IsAdmin, result.IsAdmin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetUserByEmailThrowsExceptionWhenUserDoesNotExist()
    {
        UserLogic userLogic = new UserLogic(_userMemory);
        LogIn logIn = new LogIn("test@email.com", "Mateo123.");
        var userTest = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        userLogic.GetUserByEmail(userTest.LogIn.Email);
    }
    
    [TestMethod]
    public void GetUserByIdReturnsUserPrivateDTOWhenUserExists()
    {
        var userMemory = new UserMemory();
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        var user = new User(logIn, "Andres Carsin", new DateTime(2004, 03, 10), false);
        userMemory.AddUser(user);
        var userLogic = new UserLogic(userMemory);

        var result = userLogic.GetUserById(user.Id);

        Assert.AreEqual(user.Id, result.Id);
        Assert.AreEqual(user.LogIn.Email, result.LogInDto.Email);
    }

    [TestMethod]
    public void GetUserByIdThrowsArgumentExceptionWhenUserDoesNotExist()
    {
        var userMemory = new UserMemory();
        var userLogic = new UserLogic(userMemory);

        Assert.ThrowsException<ArgumentException>(() => userLogic.GetUserById(1));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Name and Last Name are required")]
    public void CreateUser_NullNameAndLastName_ThrowsArgumentException()
    {
        UserDto userDto = new UserDto { FirstNameAndLastName = null, Birthdate = DateTime.Now, LogInDto = new LogInDto { Email = "test@example.com", Password = "password" } };
        _userLogic.CreateUser(userDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Birthdate is required")]
    public void CreateUser_NullBirthdate_ThrowsArgumentException()
    {
        UserDto userDto = new UserDto { FirstNameAndLastName = "John Doe", Birthdate = null, LogInDto = new LogInDto { Email = "test@example.com", Password = "password" } };
        _userLogic.CreateUser(userDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Email and Password are required")]
    public void CreateUser_NullLogInDto_ThrowsArgumentException()
    {
        UserDto userDto = new UserDto { FirstNameAndLastName = "John Doe", Birthdate = DateTime.Now, LogInDto = null };
        _userLogic.CreateUser(userDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Email is required")]
    public void CreateUser_EmptyEmail_ThrowsArgumentException()
    {
        UserDto userDto = new UserDto { FirstNameAndLastName = "John Doe", Birthdate = DateTime.Now, LogInDto = new LogInDto { Email = "", Password = "password" } };
        _userLogic.CreateUser(userDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Password is required")]
    public void CreateUser_EmptyPassword_ThrowsArgumentException()
    {
        UserDto userDto = new UserDto { FirstNameAndLastName = "John Doe", Birthdate = DateTime.Now, LogInDto = new LogInDto { Email = "test@example.com", Password = "" } };
        _userLogic.CreateUser(userDto);
    }

}