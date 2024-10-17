using Domain;
using Domain.Exceptions.TaskExceptions;
using Exceptions;
using Task = Domain.Task;

namespace BusinessLogic.Test;

[TestClass]
public class TaskTest
{
    private Task _task;
    
    [TestInitialize]
    public void SetUp()
    {
        _task = new Task("Title", "Description");
    }
    
    [TestMethod] 
    public void TwoParameterConstructorWorks()
    {
        Assert.AreEqual("Title", _task.Title);
        Assert.AreEqual("Description", _task.Description);
    }
    
    [TestMethod] 
    [ExpectedException(typeof(TaskTitleIsNullException))]
    public void NullTitleThrowsException()
    {
        Task task = new Task(null, "Description");
    }
    
    [TestMethod] 
    [ExpectedException(typeof(TaskDescriptionIsNullException))]
    public void NullDescriptionThrowsException()
    {
        Task task = new Task("Title", null);
    }
    
    [TestMethod] 
    public void PriorityDefaultIsUnassigned()
    {
        Assert.AreEqual(Priority.Unassigned, _task.Priority);
    }
    
    [TestMethod] 
    public void SetPriority()
    {
        _task.Priority = Priority.Low;
        
        Assert.AreEqual(Priority.Low, _task.Priority);
    }
    
    [TestMethod] 
    public void DeadlineDefaultIsToday()
    {
        Assert.AreEqual(DateTime.Today, _task.Deadline);
    }
    
    [TestMethod] 
    [ExpectedException(typeof(TaskDeadLineDateIsPreviousThanCurrentDate))]
    public void SetDeadLineBeforeTodayThrowsException()
    {
        DateTime deadline = DateTime.Today.Subtract(TimeSpan.FromDays(1));
        
        _task.Deadline = deadline;
    }
    
    [TestMethod] 
    public void SetPanel()
    {
        User user = new User();
        Panel panel = new Panel("Title", "Description", user, new Team("Any team", 3));
        
        _task.Panel = panel;
        
        Assert.AreEqual(panel.Id, _task.Panel.Id);
    }
    
    [TestMethod] 
    public void SetEliminated()
    {
        _task.Eliminated = true;
        
        Assert.AreEqual(true, _task.Eliminated);
    }

    [TestMethod]
    public void DifferentTasksHaveDifferentIds()
    {
        Task task1 = new Task("Title", "Description");
        
        Assert.AreNotEqual(_task.Id, task1.Id);
    }
    [TestMethod]
    public void AddOneComment()
    {
        User user = new User();
        Comment comment = new Comment(user, "Content");
        
        _task.AddComment(comment);
        
        Assert.AreEqual(comment, _task.Comments[0]);
    }

    [TestMethod]
    public void CommentsAreOrderedByDateAndTime()
    {
        User user = new User();
        Comment comment1 = new Comment(user,"Content");
        Thread.Sleep(1000);
        Comment comment2 = new Comment(user,"Content");
        
        _task.AddComment(comment1);
        _task.AddComment(comment2);
        
        Assert.IsTrue(_task.Comments[0].DateAndTime > _task.Comments[1].DateAndTime);
    }
    
    [TestMethod]
    [ExpectedException(typeof(TaskTitleIsBlankException))]
    public void TaskTitleIsSettingBlank()
    {
        Task task = new Task("", "Description");
    }
}