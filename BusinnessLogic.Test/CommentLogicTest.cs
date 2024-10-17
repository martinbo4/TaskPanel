using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using BusinnessLogic;
using Memory;
using Task = Domain.Task;

namespace BusinnessLogic.Test;


[TestClass]
public class CommentLogicTest
{
    private CommentLogic _commentLogic;
    
    private PanelMemory _panelMemory;
    private RecycleBinMemory _recycleBinMemory;
    private UserMemory _userMemory;
    
    private User _user;
    private Team _team1;
    private Panel _panel1;
    private Task _task1;
    private Comment _comment1;
    private RecycleBin _recycleBin;
    
    [TestInitialize]
    public void SetUp()
    {
        _panelMemory = new PanelMemory();
        _recycleBinMemory = new RecycleBinMemory();
        _userMemory = new UserMemory();
        _user = new User()
        {
            LogIn = new LogIn("example@example.com", "Mateo123."),
        };
        _team1 = new Team("Team1", 3);
        _team1.AddUserToTeam(_user);
        _panel1 = new Panel("Panel1", "Description1", _user, _team1);
        _task1 = new Task("Task1", "Description1")
        {
            Panel = _panel1
        };
        _comment1 = new Comment(_user, "Comment1");
        _task1.AddComment(_comment1);
        _panel1.AddTask(_task1);
        _commentLogic = new CommentLogic(_panelMemory, _userMemory);
        _recycleBin = new RecycleBin(_user, 10);
        _panelMemory.AddPanel(_panel1);
        _recycleBinMemory.AddRecycleBin(_recycleBin);
        _userMemory.AddUser(_user);
    }
    
    [TestMethod]
    public void CreateCommentTest()
    {
        CommentDto? commentDto = new CommentDto()
        {
            Content = "Content",
            AuthorDto = UserDto.FromUser(_user)
        };
        
        _commentLogic.CreateComment(commentDto, _task1.Id, _panel1.Id);
        
        CommentDto commentDtoAfter = _commentLogic.GetCommentById(2, _task1.Id, _panel1.Id);
        
        Assert.AreEqual(commentDto.Content, commentDtoAfter.Content);
        Assert.AreEqual(commentDto.AuthorDto.Id, commentDtoAfter.AuthorDto.Id);
    }

    [TestMethod]
    public void SolveCommentTest()
    {
        _commentLogic.SolveComment(_comment1.Id, _user.Id, _task1.Id, _panel1.Id);
        
        Assert.IsTrue(_comment1.Resolved);
        Assert.AreEqual(_comment1.Resolver.Id, _user.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Comment content cannot be empty")]
    public void CreateComment_EmptyContent_ThrowsArgumentException()
    {
        CommentDto? commentDto = new CommentDto { Content = "" };
        _commentLogic.CreateComment(commentDto, _task1.Id, _panel1.Id);
    }
}