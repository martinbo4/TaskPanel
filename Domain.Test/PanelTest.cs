using Domain;
using Task = Domain.Task;

namespace BusinessLogic.Test;



[TestClass]
public class PanelTest
{
    private User _user;
    private Panel _panel1;
    private Panel _panel2;
    private Task _task;
    private Team _team;
    
    [TestInitialize]
    public void Setup()
    {
        _user = new User();
        _team = new Team("Test Team", 3);
        _panel1 = new Panel("Name", "Description", _user, _team);
        _panel2 = new Panel("Name", "Description", _user, _team);
        _task = new Task("Name", "Description");
    }
    
    [TestMethod]
    public void ThreeParameterConstructorWorks()
    {
        Assert.AreEqual(_panel1.Name, "Name");
        Assert.AreEqual(_panel1.Description, "Description");
        Assert.AreEqual(_panel1.Creator, _user);
    }
    
    [TestMethod]
    public void FourParameterConstructorWorks()
    {
        Assert.AreEqual(_panel1.Name, "Name");
        Assert.AreEqual(_panel1.Description, "Description");
        Assert.AreEqual(_panel1.Creator, _user);
        Assert.AreEqual(_panel1.Team, _team);
    }

    [TestMethod]
    public void DifferentPanelsHaveDifferentIds()
    {
        Assert.AreNotEqual(_panel1.Id, _panel2.Id);
    }

    [TestMethod]
    public void SetTeamId()
    {
        _panel1.Team = _team;
        
        Assert.AreEqual(_team, _panel1.Team);
    }

    [TestMethod]
    public void AddOneTask()
    {
        _panel1.AddTask(_task);
        
        Assert.AreEqual(_task, _panel1.Tasks[0]);
    }

    [TestMethod]
    public void DeleteOneTask()
    {
        _panel1.AddTask(_task);
        _panel1.RemoveTask(_task.Id);
        
        Assert.AreEqual(_panel1.Tasks.Count, 0);
    }

    [TestMethod]
    public void TasksAreOrderedByPriority()
    {
        Task task = new Task("Name", "Description")
        {
            Priority = Priority.Low
        };
        _panel1.AddTask(task);
        _task.Priority = Priority.High;
        _panel1.AddTask(_task);
        
        Assert.IsTrue(_panel1.Tasks[0].Priority > _panel1.Tasks[1].Priority);
    }

    [TestMethod]
    public void TasksAreOrderedByPriorityEvenWhenPriorityChanges()
    {
        Task task = new Task("Name", "Description");
        task.Priority = Priority.Low;
        _panel1.AddTask(task);
        _task.Priority = Priority.High;
        _panel1.AddTask(_task);
        task.Priority = Priority.High;
        _task.Priority = Priority.Medium;
        
        _panel1.SortTasks();
        
        Assert.IsTrue(_panel1.Tasks[0].Priority > _panel1.Tasks[1].Priority);
    }

    [TestMethod]
    public void SetEliminated()
    {
        _panel1.Eliminated = true;
        
        Assert.IsTrue(_panel1.Eliminated);
    }

    [TestMethod]
    public void TaskHasPanelReference_When_Added()
    {
        _panel1.AddTask(_task);
        
        Assert.AreEqual(_panel1, _task.Panel);
    }
}