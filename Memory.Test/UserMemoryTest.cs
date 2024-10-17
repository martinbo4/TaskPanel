using Domain;

namespace Memory.Test;

[TestClass]
public class UserMemoryTest
{
    
    private readonly DateTime _birthdate = new DateTime(2004, 3, 5);
    private User _user;
    
    [TestInitialize]
    public void SetUp()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        _user = new User(logIn, "Mateo Pine",_birthdate,false);
        
    }
    
    [TestMethod]
    public void ElConstructorDeMemoriaUsuarioSinParametrosNoEsNull()
    {
        UserMemory userMemory = new UserMemory();
        
        Assert.IsNotNull(userMemory);
    }
    
    [TestMethod]
    public void AUserIsAddedToTheList()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();

        userMemory.AddUser(user);
        
        Assert.IsTrue(userMemory.UserList.Contains(user));
    }
    
    [TestMethod]
    public void AUserIsDeletedFromTheList()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        userMemory.AddUser(user);
        
        userMemory.DeleteUser(user);
        
        Assert.IsFalse(userMemory.UserList.Contains(user));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AttemptToDeleteAUserThatDoesNotExistInTheList()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        
        userMemory.DeleteUser(user);
    }
    
    [TestMethod]
    public void SeDevuelveElUsuarioRelacionadoAlEmail()
    {
        string email = "mateo@gmail.com";
        LogIn logIn = new LogIn(email, "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        userMemory.AddUser(user);
        
        User returnedUser = userMemory.GetUserByEmail(email);
        
        Assert.AreEqual(user, returnedUser);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AttemptToGetAUserByAnEmailThatIsNotLinkedToAnyUser()
    {
        string nonExistentEmail = "mateo123@gmail.com";
        string email = "mateo@gmail.com";
        LogIn logIn = new LogIn(email, "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        
        userMemory.AddUser(user);
        
        userMemory.GetUserByEmail(nonExistentEmail);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AttemptToGetAUserByEmailWhenTheUserIsNotInTheList()
    {
        string email = "mateo@gmail.com";
        UserMemory userMemory = new UserMemory();
        
        userMemory.GetUserByEmail(email);
    }
    
    [TestMethod]
    public void UserExistenceWithEmailWhenItExists()
    {
        string email = "mateo@gmail.com";
        LogIn logIn = new LogIn(email, "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        
        userMemory.AddUser(user);
        
        Assert.IsTrue(userMemory.UserExistence(email));
    }
    
    [TestMethod]
    public void UserExistenceWithEmailWhenItDoesNotExist()
    {
        string NonExistentEmail = "mateoNo@gmail.com";
        string email = "mateo@gmail.com";
        LogIn logIn = new LogIn(email, "Mateo123.");
        User user = new User(logIn, "Mateo Pine",_birthdate,false);
        UserMemory userMemory = new UserMemory();
        
        userMemory.AddUser(user);
        
        Assert.IsFalse(userMemory.UserExistence(NonExistentEmail));
    }
    
    [TestMethod]
    public void GetUserByIdReturnsUserWhenUserExists()
    {
        var userMemory = new UserMemory();
        LogIn logIn = new LogIn("andresCarsin@example.com", "Andres123.");
        var user = new User(logIn, "Andres Carsin", new DateTime(1999, 1, 1), false);
        userMemory.AddUser(user);

        var result = userMemory.GetUserById(user.Id);

        Assert.AreEqual(user, result);
    }

    [TestMethod]
    public void GetUserByIdThrowsArgumentExceptionWhenUserDoesNotExist()
    {
        UserMemory userMemory = new UserMemory();
        
        Assert.ThrowsException<ArgumentException>(() => userMemory.GetUserById(_user.Id));
    }
}