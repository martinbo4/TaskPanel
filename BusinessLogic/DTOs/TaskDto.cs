using Domain;
using Task = Domain.Task;

namespace BusinessLogic.DTOs;

public class TaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime Deadline { get; set; }
    public PanelDto? PanelDto { get; init; }
    public bool Eliminated { get; init; }
    public int Id { get; init; }
    public List<CommentDto> CommentsDtos { get; } = new();

    public static TaskDto FromTask(Task task)
    {
        TaskDto taskDto = new TaskDto()
        {
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Deadline = task.Deadline,
            PanelDto = PanelDto.FromPanel(task.Panel!),
            Eliminated = task.Eliminated,
            Id = task.Id
        };
        
        foreach (Comment comment in task.Comments){
            taskDto.CommentsDtos.Add(CommentDto.FromComment(comment)); 
        }
        
        return taskDto;
    }

    public Task ToTask()
    {
        Task task = new Task(Title!, Description!)
        {
            Id = Id,
            Priority = Priority,
            Deadline = Deadline,
            Panel = PanelDto!.ToPanel(),
            Eliminated = Eliminated
        };

        foreach (CommentDto commentDto in CommentsDtos)
        {
            task.AddComment(commentDto.ToComment());
        }
        
        return task;
    }
    
    public static TaskDto FromTask_When_FromPanel(Task task, PanelDto panelDto)
    {
        TaskDto taskDto = new TaskDto()
        {
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Deadline = task.Deadline,
            PanelDto = panelDto,
            Eliminated = task.Eliminated,
            Id = task.Id
        };
        
        foreach (Comment comment in task.Comments){
            taskDto.CommentsDtos.Add(CommentDto.FromComment(comment)); 
        }
        
        return taskDto;
    }
}