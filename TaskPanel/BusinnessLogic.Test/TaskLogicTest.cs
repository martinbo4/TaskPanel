using BusinessLogic.DTOs;

namespace BusinnessLogic.Test;

using BusinessLogic;
using Domain;
using Memory;

[TestClass]
public class TaskLogicTest
{
    private TaskLogic _taskLogic;
    
    private PanelMemory _panelMemory;
    private RecycleBinMemory _recycleBinMemory;
    
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
        _taskLogic = new TaskLogic(_panelMemory, _recycleBinMemory);
        _recycleBin = new RecycleBin(_user, 10);
        _panelMemory.AddPanel(_panel1);
        _recycleBinMemory.AddRecycleBin(_recycleBin);
    }

    [TestMethod]
    public void CreateTaskTest()
    {
        TaskDto? taskDto = new TaskDto()
        {
            Title = "Title",
            Description = "Description",
            Priority = Priority.High,
            Deadline = DateTime.Today
        };
        
        _taskLogic.CreateTask(taskDto, _panel1.Id);
        
        TaskDto? taskDtoAfter = _taskLogic.GetTaskById(_task1.Id+1, _panel1.Id);
        
        Assert.AreEqual(taskDto.Title, taskDtoAfter.Title);
        Assert.AreEqual(taskDto.Description, taskDtoAfter.Description);
        Assert.AreEqual(taskDto.Priority, taskDtoAfter.Priority);
        Assert.AreEqual(taskDto.Deadline, taskDtoAfter.Deadline);
        Assert.AreEqual(_panel1.Id, taskDtoAfter.PanelDto.Id);
        Assert.AreEqual(false, taskDtoAfter.Eliminated);
        Assert.AreEqual(0, taskDtoAfter.CommentsDtos.Count);
    }
    
    [TestMethod]
    public void ModifyTaskTest()
    {
        _panel1.AddTask(_task1);
        TaskDto? taskDto = _taskLogic.GetTaskById(_task1.Id, _panel1.Id);
        taskDto.Title = "New Title";
        taskDto.Description = "New Description";
        
        _taskLogic.ModifyTask(taskDto);
        
        Assert.AreEqual(_task1.Title, "New Title");
        Assert.AreEqual(_task1.Description, "New Description");
    }
    
    [TestMethod]
    public void DeleteTaskTest()
    {
        TaskDto? taskDto = _taskLogic.GetTaskById(_task1.Id, _panel1.Id);
        
        _taskLogic.DeleteTask(taskDto, UserDto.FromUser(_user), false);
        
        Assert.IsTrue(_recycleBinMemory.GetRecycleBinByUser(_user).Tasks.Any(t => t.Id == _task1.Id));
        Assert.IsTrue(_panelMemory.GetPanelById(_panel1.Id).Tasks.All(t => t.Id != _task1.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Title is required")]
    public void CreateTask_EmptyTitle_ThrowsArgumentException()
    {
        TaskDto? taskDto = new TaskDto { Title = "", Description = "Task description", };
        _taskLogic.CreateTask(taskDto, _panel1.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Description is required")]
    public void CreateTask_EmptyDescription_ThrowsArgumentException()
    {
        TaskDto? taskDto = new TaskDto { Title = "Task title", Description = "" };
        _taskLogic.CreateTask(taskDto, _panel1.Id);
    }
}