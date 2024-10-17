namespace Domain;

public class Panel
{
    public int Id { get; private set; }
    private static int _idCounter = 1;
    public string Name { get; set; }
    public string Description { get; set; }
    public User Creator { get; }
    public Team Team { get; set; }
    public bool Eliminated { get; set; }
    public bool IsOverdueTaskPanel { get; init; }
    public List<Task> Tasks { get; } = new();
    
    public Panel(string name, string description, User creator, Team team)
    {
        Name = name;
        Description = description;
        Creator = creator;
        Team = team;
        Id = _idCounter++;
    }

    public void AddTask(Task task)
    {
        if (!IsOverdueTaskPanel) task.OriginalPanel = this;
        task.Panel = this;
        Tasks.Add(task);
        SortTasks();
    }
    
    public void RemoveTask(int taskId)
    {
        Tasks.Remove(Tasks.First(t => t.Id == taskId));
    }

    public void SortTasks()
    {
        Tasks.Sort();
        Tasks.Reverse();
    }

    public void SetId(int id)
    {
        Id = id;
        _idCounter = Math.Max(_idCounter, id + 1);
    }
}