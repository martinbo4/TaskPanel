using Domain;
using Domain.Exceptions.RecycleBinExceptions;
using Task = Domain.Task;

namespace BusinessLogic.Test;

[TestClass]
public class RecycleBinTest
{
    private const string UserName = "Andres";
    private static readonly DateTime _birthdate = new DateTime(2003, 05, 23);
    private const bool IsAdmin = false;
    private const string Content = "This is an example comment.";
    private const int MaxCapacity = 10;
    private User _user;
    
    [TestInitialize]
    public void Setup()
    {
        LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
        _user = new User(logIn, UserName, _birthdate, IsAdmin); 
    }
    
    [TestMethod]
    public void RecycleBinConstructorIsNotNull()
    {
        RecycleBin recycleBin = new RecycleBin(_user);
        
        Assert.IsNotNull(recycleBin);
    }
    
    [TestMethod]
    public void RecycleBinConstructorSetsUser()
    {
        RecycleBin recycleBin = new RecycleBin(_user);
        
        Assert.AreEqual(_user, recycleBin.User);
    }
    
    [TestMethod]    
    public void RecycleBinConstructorWithTwoParametersIsNotNull()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        
        Assert.IsNotNull(recycleBin);
    }
    
    [TestMethod]
    public void RecycleBinConstructorWithTwoParametersSetsUser()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        
        Assert.AreEqual(_user, recycleBin.User);
    }
    
    [TestMethod]
    public void RecycleBinSetMaxCapacity()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        
        Assert.AreEqual(MaxCapacity, recycleBin.MaxCapacity);
    }
    
    [TestMethod]
    public void RecycleBinAddPanel()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        Panel panel = new Panel("Panel 1", "This is a panel", _user, new Team("Any team", 3));
        
        recycleBin.AddPanel(panel);
        
        Assert.AreEqual(1, recycleBin.Panels.Count);
    }
    
    [TestMethod]
    public void RecycleBinRemovePanel()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        Panel panel = new Panel("Panel 1", "This is a panel", _user, new Team("Any team", 3));
        recycleBin.AddPanel(panel);
        
        recycleBin.RemovePanel(panel.Id);
        
        Assert.AreEqual(0, recycleBin.Panels.Count);
    }
    
    [TestMethod]
    public void RecycleBinAddTask()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        Task task = new Task("Task 1", "This is a task");
        
        recycleBin.AddTask(task);
        
        Assert.AreEqual(1, recycleBin.Tasks.Count);
    }
    
    [TestMethod]
    public void RecycleBinRemoveTask()
    {
        RecycleBin recycleBin = new RecycleBin(_user, MaxCapacity);
        Task task = new Task("Task 1", "This is a task");
        recycleBin.AddTask(task);
        
        recycleBin.RemoveTask(task.Id);
        
        Assert.AreEqual(0, recycleBin.Tasks.Count);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RecycleBinExceedCapacityException))]
    public void RecycleBinAddMoreTasksThanMaxCapacity()
    {
        RecycleBin recycleBin = new RecycleBin(_user, 1);
        Task task1 = new Task("Task 1", "This is a task");
        Task task2 = new Task("Task 2", "This is a task");
        
        recycleBin.AddTask(task1);
        recycleBin.AddTask(task2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RecycleBinExceedCapacityException))]
    public void RecycleBinAddMorePanelThanMaxCapacity()
    {
        RecycleBin recycleBin = new RecycleBin(_user, 1);
        Panel panel1 = new Panel("Panel 1", "This is a panel", _user, new Team("Any team", 3));
        Panel panel2 = new Panel("Panel 2", "This is a panel", _user, new Team("Any team", 3));
        
        recycleBin.AddPanel(panel1);
        recycleBin.AddPanel(panel2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RecycleBinExceedCapacityException))]
    public void RecycleBinAddMorePanelsAndTasksThanMaxCapacity()
    {
        RecycleBin recycleBin = new RecycleBin(_user, 2);
        Panel panel1 = new Panel("Panel 1", "This is a panel", _user, new Team("Any team", 3));
        Task task1 = new Task("Task 1", "This is a task");
        Panel panel2 = new Panel("Panel 2", "This is a panel", _user, new Team("Any team", 3));
        
        recycleBin.AddPanel(panel1);
        recycleBin.AddTask(task1);
        recycleBin.AddPanel(panel2);
    }
}