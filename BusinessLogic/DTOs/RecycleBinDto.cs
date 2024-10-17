using Domain;
using Task = Domain.Task;

namespace BusinessLogic.DTOs;

public class RecycleBinDto
{
    public UserDto? UserDto { get; init; }
    public List<PanelDto> PanelsDtos { get; init; } = new();
    public List<TaskDto> TasksDtos { get; init; } = new();
    public int MaxCapacity { get; init; }

    public static RecycleBinDto FromRecycleBin(RecycleBin recycleBin)
    {
        RecycleBinDto dto = new()
        {
            UserDto = UserDto.FromUser(recycleBin.User),
            MaxCapacity = recycleBin.MaxCapacity
        };
        foreach (Panel panel in recycleBin.Panels)
        {
            dto.PanelsDtos.Add(PanelDto.FromPanel(panel));    
        }
        foreach (Task task in recycleBin.Tasks)
        {
            dto.TasksDtos.Add(TaskDto.FromTask(task));    
        }
        return dto;
    }
    
    public RecycleBin ToRecycleBin()
    {
        RecycleBin recycleBin = new RecycleBin(UserDto!.ToUser(), MaxCapacity);
        foreach(PanelDto panelDto in PanelsDtos)
        {
            recycleBin.AddPanel(panelDto.ToPanel());
        }
        foreach(TaskDto taskDto in TasksDtos)
        {
            recycleBin.AddTask(taskDto.ToTask());
        }
        return recycleBin;
    }
}