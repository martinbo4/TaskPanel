using Domain.Exceptions.TaskExceptions;
using Exceptions;

namespace Domain;

public enum Priority { Unassigned, Low, Medium, High }

public class Task : IComparable<Task>
{
    public int Id { get; init; }
    private static int _idCounter = 1;
    public string Title { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    private DateTime _deadline;
    public DateTime Deadline
    {
        get => _deadline;
        set => ValidateDeadline(value);
    }
    public Panel? Panel { get; set; }
    public Panel? OriginalPanel { get; set; }
    public bool Eliminated { get; set; }

    public List<Comment> Comments { get; init; } = new();
    public Task(String title, String description)
    {
        switch (title)
        {
            case null:
                throw new TaskTitleIsNullException("The title of the task is null");
            case "":
                throw new TaskTitleIsBlankException("The title of the task is empty");
        }

        Title = title;
        Description = description ?? throw new TaskDescriptionIsNullException("The description of the task is null");
        Deadline = DateTime.Today;
        Id = _idCounter++;
    }
    
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
        Comments.Sort();
        Comments.Reverse();
    }
    
    private void ValidateDeadline(DateTime deadline)
    {
        if (deadline < DateTime.Today)
        {
            throw new TaskDeadLineDateIsPreviousThanCurrentDate("The deadline of the task is previous than the current date");
        }
        _deadline = deadline;
    }
    
    public int CompareTo(Task? other)
    {
        if (other == null) return 1;
        int priorityComparison = Priority.CompareTo(other.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }
        return -Deadline.CompareTo(other.Deadline);
    }
}