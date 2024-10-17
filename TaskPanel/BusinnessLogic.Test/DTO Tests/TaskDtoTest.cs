using BusinessLogic.DTOs;
using Domain;
using Task = Domain.Task;

namespace BusinnessLogic.Test.DTO_Tests;

[TestClass]
public class TaskDtoTest
{
    private User _user;
    private Comment _comment1;
    private Comment _comment2;
    private Team _team;
    private Panel _panel;
    
    [TestInitialize]
    public void Setup()
    {
        _user = new User()
        {
            LogIn = new LogIn("example@example.com", "Mateo123."),
        };
        _comment1 = new Comment(_user, "Content");
        _comment2 = new Comment(_user, "Content");
        _team = new Team("Example Team", 2);
        _team.AddUserToTeam(_user);
        _panel = new Panel("Title", "Description", _user, _team);
    }
    [TestMethod]
    public void FromTask_ShouldMapCorrectly()
    {
        List<Comment> comments = new List<Comment>() { _comment1, _comment2 };
        Task task = new Task("Task", "Description")
        {
            Comments = comments,
            Deadline = DateTime.Now,
            Eliminated = false,
            Panel = _panel,
            Priority = Priority.High
        };
        
        TaskDto taskDto = TaskDto.FromTask(task);
        
        Assert.AreEqual(task.Title, taskDto.Title);
        Assert.AreEqual(task.Description, taskDto.Description);
        Assert.AreEqual(task.Id, taskDto.Id);
        Assert.AreEqual(task.Priority, taskDto.Priority);
        Assert.AreEqual(task.Eliminated, taskDto.Eliminated);
        Assert.AreEqual(task.Deadline, taskDto.Deadline);
        Assert.AreEqual(task.Panel.Id, taskDto.PanelDto.Id);
        foreach (Comment comment in task.Comments)
        {
            Assert.IsTrue(taskDto.CommentsDtos.Any(c => c.Id == comment.Id));
        }

    }

    [TestMethod]
    public void ToTask_ShouldMapCorrectly()
    {
        List<Comment> comments = new List<Comment>() { _comment1, _comment2 };

        TaskDto taskDto = new TaskDto()
        {
            Id = 1,
            Title = "Task",
            Description = "Description",
            Deadline = DateTime.Now,
            Priority = Priority.High,
            PanelDto = PanelDto.FromPanel(_panel),
            Eliminated = false
        };
        foreach (Comment comment in comments)
        {
            taskDto.CommentsDtos.Add(CommentDto.FromComment(comment));
        }
        
        Task task = taskDto.ToTask();
        
        Assert.AreEqual(task.Title, taskDto.Title);
        Assert.AreEqual(task.Description, taskDto.Description);
        Assert.AreEqual(task.Id, taskDto.Id);
        Assert.AreEqual(task.Priority, taskDto.Priority);
        Assert.AreEqual(task.Eliminated, taskDto.Eliminated);
        Assert.AreEqual(task.Deadline, taskDto.Deadline);
        Assert.AreEqual(task.Panel.Id, taskDto.PanelDto.Id);
        foreach (Comment comment in task.Comments)
        {
            Assert.IsTrue(taskDto.CommentsDtos.Any(c => c.Id == comment.Id));
        }
    }
}