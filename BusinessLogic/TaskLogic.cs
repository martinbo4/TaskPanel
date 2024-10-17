using BusinessLogic.DTOs;

namespace BusinessLogic;

using Memory;
using Domain;
public class TaskLogic
{
    private readonly PanelMemory _panelMemory;
    private readonly RecycleBinMemory _recycleBinMemory;
    
    public TaskLogic(PanelMemory panelMemory, RecycleBinMemory recycleBinMemory)
    {
        _panelMemory = panelMemory;
        _recycleBinMemory = recycleBinMemory;
    }

    public void CreateTask(TaskDto? taskDto, int panelId)
    {
        ValidateTaskDto(taskDto);
        Task task = new Task(taskDto.Title!, taskDto.Description!)
        {
            Priority = taskDto.Priority,
            Deadline = taskDto.Deadline,
            Panel = _panelMemory.GetPanelById(panelId),
            Eliminated = false
        };
        _panelMemory.GetPanelById(panelId).AddTask(task);
    }
    
    public TaskDto? GetTaskById(int taskId, int panelId)
    {
        Panel panel = _panelMemory.GetPanelById(panelId);
        Task task = panel.Tasks.Find(t => t.Id == taskId)!;
        return TaskDto.FromTask(task);
    }
    
    public void ModifyTask(TaskDto? taskDto)
    {
        ValidateTaskDto(taskDto);
        Panel panel = _panelMemory.GetPanelById(taskDto.PanelDto!.Id);
        Task task = panel.Tasks.Find(t => t.Id == taskDto.Id)!;
        task.Title = taskDto.Title!;
        task.Description = taskDto.Description!;
        task.Priority = taskDto.Priority;
        panel.SortTasks();
    }
    
    public void DeleteTask(TaskDto? taskDto, UserDto user, bool permanently)
    {
        Panel panel = _panelMemory.GetPanelById(taskDto.PanelDto!.Id);
        Task task = panel.Tasks.Find(t => t.Id == taskDto.Id)!;
        if(permanently)
        {
            panel.Tasks.Remove(task);
        } 
        else
        {
            RecycleBin recycleBin = _recycleBinMemory.GetRecycleBinByUser(user.ToUser());
            recycleBin.AddTask(task);
            task.Eliminated = true;
            panel.Tasks.Remove(task);
        }
    }

    public void UpdateTasks()
    {
        foreach (Panel panel in _panelMemory.PanelList)
        {
            List<Task> newOverdueTasks = new List<Task>();
            if (!panel.IsOverdueTaskPanel)
            {
                Team team = panel.Team;
                List<Panel> panels = team.Panels;
                foreach (Task task in panel.Tasks)
                {
                    if (task.Deadline < DateTime.Now)
                    {
                        newOverdueTasks.Add(task);
                        panels.Find(p => p.IsOverdueTaskPanel)!.AddTask(task);
                    }
                }
                foreach (Task task in newOverdueTasks)
                {
                    panel.RemoveTask(task.Id);
                }
            }
        }
    }

    public void ReactivateTask(TaskDto? taskDto)
    {
        Panel panel = _panelMemory.GetPanelById(taskDto.PanelDto!.Id);
        Task task = panel.Tasks.Find(t => t.Id == taskDto.Id)!;
        task.Deadline = taskDto.Deadline;
        panel.RemoveTask(task.Id);
        task.OriginalPanel!.AddTask(task);
    }

    private static void ValidateTaskDto(TaskDto? taskDto)
    {
        if (string.IsNullOrWhiteSpace(taskDto.Title))
        {
            throw new ArgumentException("Title is required");
        }
        if (string.IsNullOrWhiteSpace(taskDto.Description))
        {
            throw new ArgumentException("Description is required");
        }
    }
}
