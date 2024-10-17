using BusinessLogic.DTOs;
using Domain;

namespace BusinnessLogic.Test.DTO_Tests;

[TestClass]
public class LogInDtoTest
{
    [TestMethod]
    public void FromLogIn_ShouldMapCorrectly()
    {
        LogIn login = new LogIn("example@example.com", "Mateo123.");
        
        LogInDto dto = LogInDto.FromLogIn(login);
        
        Assert.AreEqual(login.Email, dto.Email);
        Assert.AreEqual(login.Password, dto.Password);
    }

    [TestMethod]
    public void ToLogIn_ShouldMapCorrectly()
    {
        LogInDto dto = new LogInDto()
        {
            Email = "example@example.com",
            Password = "Mateo123."
        };

        LogIn logIn = dto.ToLogIn();
        
        Assert.AreEqual(logIn.Email, dto.Email);
        Assert.AreEqual(logIn.Password, dto.Password);
    }
}