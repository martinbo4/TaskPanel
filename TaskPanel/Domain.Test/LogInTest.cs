using Domain;



namespace BusinessLogic.Test;

[TestClass]
public class LogInTest
{
    [TestMethod]
    public void ConstructorWithTwoParametersLogInIsNotNull()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        
        Assert.IsNotNull(logIn);
    }
    
    [TestMethod]
    public void ConstructorWithTwoParametersLogInLoadsDataCorrectly()
    {
        string email = "mateo@gmail.com";
        string password = "Mateo123.";
        
        LogIn logIn = new LogIn(email, password);

        Assert.AreEqual(email, logIn.Email);
        Assert.AreEqual(password, logIn.Password);
    }
    
    [TestMethod]
    public void AValidPasswordIsEntered()
    {
        string password = "Mateo123.";

        LogIn.ValidatePassword(password);
        
        Assert.IsTrue(true);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void APasswordShorterThanAllowedIsEntered()
    {
        string password = "Mate12.";
        
        LogIn.ValidatePassword(password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void APasswordWithNoUppercaseLetterIsEntered()
    {
        string password = "mateo123.";
        
        LogIn.ValidatePassword(password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void APasswordWithNoLowercaseLetterIsEntered()
    {
        string password = "MATEO123.";
        
        LogIn.ValidatePassword(password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void APasswordWithNoNumberIsEntered()
    {
        string password = "MateoPine.";
        
        LogIn.ValidatePassword(password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void APasswordWithNoSpecialCharacterIsEntered()
    {
        string password = "Mate1234";
        
        LogIn.ValidatePassword(password);
    }
    
    [TestMethod]
    public void AValidEmailIsEntered()
    {
        string email = "mate@gmail.com";

        LogIn.ValidateEmail(email);
        Assert.IsTrue(true);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnEmailWithNoUsernameIsEntered()
    {
        string email = "@gmail.com";
        
        LogIn.ValidateEmail(email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnEmailWithNothingAfterTheDotIsEntered()
    {
        string email = "mateo@gmail.";
        
        LogIn.ValidateEmail(email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnEmailWithNoDomainIsEntered()
    {
        string email = "mateo@";
        
        LogIn.ValidateEmail(email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnEmailWithNoSymbolsIsEntered()
    {
        string email = "mateogmailcom";
            
        LogIn.ValidateEmail(email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AnEmailLongerThanAllowedIsEntered()
    {
        string email = "mateomateomateooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo@gmail.com";
        
        LogIn.ValidateEmail(email);
    }
}