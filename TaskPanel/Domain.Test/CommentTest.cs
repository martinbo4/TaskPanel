using Domain;
using Domain.Exceptions.CommentExceptions;

namespace BusinessLogic.Test;

[TestClass]
public class CommentTest
{
    private const string UserName = "Andres";
    private static readonly DateTime Birthdate = new DateTime(2003, 05, 23);
    private const bool IsAdmin = false;
    private const string Content = "This is an example comment.";
    private User _author;
    
    [TestInitialize]
    public void Setup()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        _author = new User(logIn, UserName, Birthdate, IsAdmin); 
    }
    
    [TestMethod]
    public void CommentConstructorWithTwoParametersIsNotNull()
    {
        Comment comment = new Comment(_author, Content);
        
        Assert.IsNotNull(comment);
    }
    
    [TestMethod]
    public void CommentConstructorWithTwoParametersSetsAuthor()
    {
        Comment comment = new Comment(_author, Content);
        
        Assert.AreEqual(_author, comment.Author);
    }
    
    [TestMethod]
    public void CommentConstructorWithTwoParametersSetsContent()
    {
        Comment comment = new Comment(_author, Content);
        
        Assert.AreEqual(Content, comment.Content);
    }
    
    [TestMethod]
    public void CommentConstructorWithTwoParametersSetsDateAndTime()
    {
        Comment comment = new Comment(_author, Content);
        
        Assert.IsTrue(comment.DateAndTime > DateTime.Now.AddSeconds(-1));
    }
    
    [TestMethod]
    public void CommentConstructorWithTwoParametersSetsResolvedAsFalse()
    {
        Comment comment = new Comment(_author, Content);
        
        Assert.IsFalse(comment.Resolved);
    }
    
    [TestMethod]
    public void CommentSetsResolvedAsTrue()
    {
        Comment comment = new Comment(_author, Content);
        
        comment.Solve();
        
        Assert.IsTrue(comment.Resolved);
    }
    
    [TestMethod]
    public void CommentSetsResolverCorrectly()
    {
        User resolver = new User(new LogIn("mateo@gmail.com", "Mateo123."), "Pepe  ", Birthdate, IsAdmin);
        Comment comment = new Comment(_author, Content);
        
        comment.SetResolver(resolver);
        
        Assert.AreEqual(resolver, comment.Resolver);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CommentResolverIsNullException))]
    public void CommentSetsResolverCorrectlyWhenResolverIsNull()
    {
        Comment comment = new Comment(_author, Content);
        
        comment.SetResolver(null);
    }
    
    [TestMethod]
    public void CommentSetResolutionDateTimeCorrectly()
    {
        Comment comment = new Comment(_author, Content);
        
        comment.Solve();
        
        Assert.IsTrue(comment.ResolutionDateTime < DateTime.Now.AddSeconds(1));
    }
}