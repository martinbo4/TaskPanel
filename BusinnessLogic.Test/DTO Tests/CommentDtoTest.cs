using BusinessLogic.DTOs;
using Domain;

namespace BusinnessLogic.Test.DTO_Tests;

[TestClass]
public class CommentDtoTest
{
    [TestMethod]
    public void FromComment_ShouldMapCorrectly()
    {
        User user  = new()
        {
            LogIn = new LogIn("example@email.com", "Mateo123."),
        };
        Comment comment = new Comment (user, "Test content");

        CommentDto commentDto = CommentDto.FromComment(comment);

        Assert.AreEqual(comment.Id, commentDto.Id);
        Assert.AreEqual(comment.Content, commentDto.Content);
        Assert.AreEqual(comment.Author.Id, commentDto.AuthorDto.Id);
        Assert.AreEqual(comment.DateAndTime, commentDto.DateAndTime);
        Assert.AreEqual(comment.ResolutionDateTime, commentDto.ResolutionDateTime);
        Assert.AreEqual(comment.Resolved, commentDto.Resolved);
        Assert.AreEqual(comment.Resolver, commentDto.ResolverDto);
    }
    
    [TestMethod]
    public void ToComment_ShouldMapCorrectly()
    {
        User user  = new()
        {
            LogIn = new LogIn("example@email.com", "Mateo123."),
        };
        CommentDto commentDto = new CommentDto
        {
            Content = "Test content",
            AuthorDto = UserDto.FromUser(user),
            DateAndTime = DateTime.Now,
            ResolutionDateTime = DateTime.Now,
            Resolved = true
        };

        Comment comment = commentDto.ToComment();

        Assert.AreEqual(commentDto.Id, comment.Id);
        Assert.AreEqual(commentDto.Content, comment.Content);
        Assert.AreEqual(commentDto.AuthorDto.Id, comment.Author.Id);
        Assert.AreEqual(commentDto.DateAndTime, comment.DateAndTime);
        Assert.AreEqual(commentDto.ResolutionDateTime, comment.ResolutionDateTime);
        Assert.AreEqual(commentDto.Resolved, comment.Resolved);
        Assert.AreEqual(commentDto.ResolverDto, comment.Resolver);
    }
    
    [TestMethod]
    public void FromComment_ShouldMapCorrectly_When_ResolverNotNull()
    {
        User user  = new()
        {
            LogIn = new LogIn("example@email.com", "Mateo123."),
        };
        Comment comment = new Comment (user, "Test content");
        comment.Solve();
        comment.SetResolver(user);
        
        CommentDto commentDto = CommentDto.FromComment(comment);

        Assert.AreEqual(comment.Id, commentDto.Id);
        Assert.AreEqual(comment.Content, commentDto.Content);
        Assert.AreEqual(comment.Author.Id, commentDto.AuthorDto.Id);
        Assert.AreEqual(comment.DateAndTime, commentDto.DateAndTime);
        Assert.AreEqual(comment.ResolutionDateTime, commentDto.ResolutionDateTime);
        Assert.AreEqual(comment.Resolved, commentDto.Resolved);
        Assert.AreEqual(comment.Resolver.Id, commentDto.ResolverDto.Id);
    }
}