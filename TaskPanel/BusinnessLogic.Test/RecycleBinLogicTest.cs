using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using Memory;
using Task = Domain.Task;

namespace BusinnessLogic.Test;

[TestClass]
public class RecycleBinLogicTest
{
    private RecycleBinLogic _recycleBinLogic;
    private TaskLogic _taskLogic;
    private PanelLogic _panelLogic;
    
    private PanelMemory _panelMemory;
    private RecycleBinMemory _recycleBinMemory;
    private UserMemory _userMemory;
    private TeamMemory _teamMemory;
    
    private User _user;
    private Team _team1;
    private Panel _panel1;
    private Task _task1;
    private Comment _comment1;

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
        _userMemory.AddUser(_user);
        _panelMemory.AddPanel(_panel1);
        _recycleBinLogic = new RecycleBinLogic(_recycleBinMemory, _userMemory);
        _taskLogic = new TaskLogic(_panelMemory, _recycleBinMemory);
        _panelLogic = new PanelLogic(_panelMemory, _recycleBinMemory, _teamMemory);
    }
    
    [TestMethod]
    public void RecycleBinIsCreatedIfDoesNotExist_When_GetRecylceBin()
    {
        RecycleBinDto dto = _recycleBinLogic.GetRecycleBin(_user.Id);
        
        Assert.AreEqual(_recycleBinMemory.RecycleBins.Count, 1);
    }
    
    [TestMethod]
    public void EmptyRecycleBinTest()
    {
        _recycleBinLogic.EmptyRecycleBin(_user.Id);
        
        Assert.AreEqual(_recycleBinMemory.GetRecycleBinByUser(_user).Panels.Count, 0);
        Assert.AreEqual(_recycleBinMemory.GetRecycleBinByUser(_user).Panels.Count, 0);
    }
    
    [TestMethod]
    public void RemoveTaskFromRecycleBin()
    {
        _taskLogic.DeleteTask(TaskDto.FromTask(_task1), UserDto.FromUser(_user), false);
        
        _recycleBinLogic.RemoveTask(TaskDto.FromTask(_task1), _user.Id);
        
        Assert.IsTrue(_recycleBinMemory.GetRecycleBinByUser(_user).Tasks.Count == 0);
    }
    
    [TestMethod]
    public void RemovePanelFromRecycleBin()
    {
        _panelLogic.DeletePanel(PanelDto.FromPanel(_panel1), UserDto.FromUser(_user), false);
        
        _recycleBinLogic.RemovePanel(PanelDto.FromPanel(_panel1), _user.Id);
        
        Assert.IsTrue(_recycleBinMemory.GetRecycleBinByUser(_user).Panels.Count == 0);
    }
}