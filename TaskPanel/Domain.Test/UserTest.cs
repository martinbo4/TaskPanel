using Domain;

namespace BusinessLogic.Test;

[TestClass]
public class UserTest
{
    private readonly DateTime _birthdate = new DateTime(2004, 3, 5);

    [TestMethod]
    public void UserConstructorWithTwoParametersIsNotNull()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        bool isAdmin = false;
        string firstNameAndLastName = "Mateo Pine";
        User user = new User(logIn, firstNameAndLastName, _birthdate, isAdmin);
        
        Assert.IsNotNull(user);
    }
    
    [TestMethod]
    public void UserConstructorWithTwoParametersIsNotNullAndIsAdmin()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        bool isAdmin = true;
        string firstNameAndLastName = "Mateo Pine";
        User user = new User(logIn, firstNameAndLastName, _birthdate, isAdmin);
        
        Assert.IsNotNull(user);
    }
    
    [TestMethod]
    public void UserConstructorWithTwoParametersLoadsDataCorrectly()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        string firstNameAndLastName = "MateoPine";
        bool isAdmin = false;
        User user = new User(logIn, firstNameAndLastName, _birthdate, isAdmin);
        
        Assert.AreEqual(logIn, user.LogIn);
        Assert.AreEqual(_birthdate, user.Birthdate);
        Assert.AreEqual(firstNameAndLastName, user.FirstNameAndLastName);
        Assert.AreEqual(isAdmin, user.IsAdmin);
    }
    
    [TestMethod]
    public void UserEntersCorrectFirstNameAndLastName()
    {
        string firstNameAndLastName = "Mateo Pine"; 
        
        User.ValidateFirstNameAndLastName(firstNameAndLastName);
        
        Assert.IsTrue(true);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FirstNameAndLastNameIsNull()
    {
        string firstNameAndLastName = null;

        User.ValidateFirstNameAndLastName(firstNameAndLastName);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UserEntersFirstNameAndLastNameWithExceedingLength()
    {
        string firstNameAndLastName = "Mateoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo Pineeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        
        User.ValidateFirstNameAndLastName(firstNameAndLastName);
    }
    
    [TestMethod]
    public void UserEntersCorrectBirthDate()
    {
        User.ValidateBirthDate(_birthdate);
        
        Assert.IsTrue(true);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UserEntersIncorrectBirthDate()
    {
        DateTime birthdate = new DateTime(2034, 3, 5);
        
        User.ValidateBirthDate(birthdate);
    }
    
    [TestMethod]
    public void UserIdIncrementsWithEachUserCreation()
    {
        LogIn logIn1 = new LogIn("user1@gmail.com", "User123.");
        LogIn logIn2 = new LogIn("user2@gmail.com", "User123.");
        
        User user1 = new User(logIn1, "User One", new DateTime(2000, 1, 1), false);
        User user2 = new User(logIn2, "User Two", new DateTime(2001, 1, 1), false);
    
        Assert.AreEqual(user1.Id + 1, user2.Id);
    }
}