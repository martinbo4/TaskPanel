using Domain;
using Task = Domain.Task;

namespace BusinessLogic.DTOs;

public class PanelDto
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public UserDto? CreatorDto { get; set; }
    public TeamDto? TeamDto { get; set; }
    public List<TaskDto> TasksDtos { get; private init; } = new();
    public bool Eliminated { get; init; }
    public bool IsOverdueTaskPanel { get; private init; }

    public static PanelDto FromPanel(Panel panel)
    {
        PanelDto dto = new PanelDto()
        {
            Id = panel.Id,
            Name = panel.Name,
            Description = panel.Description,
            CreatorDto = UserDto.FromUser(panel.Creator),
            TasksDtos = new List<TaskDto>(),
            Eliminated = panel.Eliminated,
            IsOverdueTaskPanel = panel.IsOverdueTaskPanel
        };
        dto.TeamDto = TeamDto.FromTeam_When_FromPanel(panel.Team, dto); 
        foreach (Task task in panel.Tasks)
        {
            dto.TasksDtos.Add(TaskDto.FromTask_When_FromPanel(task, dto));
        }
        return dto;
    }
    
    public Panel ToPanel()
    {
        Panel panel = new Panel(Name!, Description!, CreatorDto!.ToUser(), TeamDto!.ToTeam())
        {
            Eliminated = Eliminated,
            IsOverdueTaskPanel = IsOverdueTaskPanel
        };
        panel.SetId(Id);
        return panel;
    }
}