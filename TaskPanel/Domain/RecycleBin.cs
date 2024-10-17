using Domain.Exceptions.RecycleBinExceptions;

namespace Domain;

public class RecycleBin
{
    public User User { get; }
    public List<Panel> Panels { get; init; } = new();
    public List<Task> Tasks { get; init; } = new();
    public int MaxCapacity { get; }
    
    public RecycleBin(User user)
    {
        User = user;
    }

    public RecycleBin(User user, int maxCapacity)
    {
        User = user;
        Panels = new List<Panel>();
        Tasks = new List<Task>();
        MaxCapacity = maxCapacity;
    }
    
    public void AddPanel(Panel panel)
    {
        if (PanelsAndTasksExceedMaxCapacity())
        {
            throw new RecycleBinExceedCapacityException("Panels exceed max capacity");
        }
        Panels.Add(panel);
    }

    public void RemovePanel(int panelId)
    {
        Panel panel = Panels.FirstOrDefault(p => p.Id == panelId) ?? throw new ArgumentException("Panel not found in recycle bin");
        Panels.Remove(panel);
    }

    public void AddTask(Task task)
    {
        if(PanelsAndTasksExceedMaxCapacity())
        {
            throw new RecycleBinExceedCapacityException("Tasks exceed max capacity");
        }
        Tasks.Add(task);
    }
    
    public void RemoveTask(int taskId)
    {
        Task task = Tasks.FirstOrDefault(t => t.Id == taskId) ?? throw new ArgumentException("Task not found in recycle bin");
        if (task is null)
        {
            throw new ArgumentException("Task not found in recycle bin");
        }
        Tasks.Remove(task);
    }
    
    private bool PanelsAndTasksExceedMaxCapacity()
    {
        return Panels.Count + Tasks.Count >= MaxCapacity;
    }
}