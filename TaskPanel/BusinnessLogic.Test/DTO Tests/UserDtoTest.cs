using BusinessLogic.DTOs;
using Domain;

namespace BusinnessLogic.Test.DTO_Tests;

[TestClass]
public class UserDtoTest
{
    [TestMethod]
    public void FromUser_ShouldMapCorrectly()
    {
        User user = new User
        (
            new LogIn("mateo@gmail.com", "Mateo123."), 
            "Name LastName", 
            new DateTime(2004, 03, 10),
            false
        );
        UserDto userDto = UserDto.FromUser(user);
        
        Assert.AreEqual(user.Id, userDto.Id);
        Assert.AreEqual(user.LogIn.Email, userDto.LogInDto.Email);
        Assert.AreEqual(user.LogIn.Password, userDto.LogInDto.Password);
        Assert.AreEqual(user.FirstNameAndLastName, userDto.FirstNameAndLastName);
        Assert.AreEqual(user.Birthdate, userDto.Birthdate);
        Assert.AreEqual(user.IsAdmin, userDto.IsAdmin);
    }
    
    [TestMethod]
    public void ToUser_ShouldMapCorrectly()
    {
        UserDto userDto = new UserDto()
        {
            Id = 1,
            LogInDto = new LogInDto(){ Email = "mateo@gmail.com", Password = "Mateo123." },
            Birthdate = DateTime.Today,
            FirstNameAndLastName = "Jane Doe",
            IsAdmin = false
        };

        User userMapped = userDto.ToUser();
        
        Assert.AreEqual(userMapped.Id, userDto.Id);
        Assert.AreEqual(userMapped.LogIn.Email, userDto.LogInDto.Email);
        Assert.AreEqual(userMapped.LogIn.Password, userDto.LogInDto.Password);
        Assert.AreEqual(userMapped.FirstNameAndLastName, userDto.FirstNameAndLastName);
        Assert.AreEqual(userMapped.Birthdate, userDto.Birthdate);
        Assert.AreEqual(userMapped.IsAdmin, userDto.IsAdmin);
    }
}