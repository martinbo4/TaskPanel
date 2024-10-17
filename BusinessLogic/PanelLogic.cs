using BusinessLogic.DTOs;
using Domain;
using Memory;
using Task = Domain.Task;

namespace BusinessLogic;

public class PanelLogic
{
    private readonly PanelMemory _panelMemory;
    private readonly RecycleBinMemory _recycleBinMemory;
    private readonly TeamMemory _teamMemory;
    
    public PanelLogic(PanelMemory panelMemory, RecycleBinMemory recycleBinMemory, TeamMemory teamMemory)
    {
        _panelMemory = panelMemory;
        _recycleBinMemory = recycleBinMemory;
        _teamMemory = teamMemory;
    }

    public void CreatePanel(PanelDto? dto)
    {
        ValidatePanelDto(dto);
        Team team = _teamMemory.GetTeamById(dto.TeamDto!.Id);
        Panel panel = new Panel(dto.Name!, dto.Description!, dto.CreatorDto!.ToUser(), team)
        {
            Eliminated = dto.Eliminated
        };
        team.AddPanel(panel);
        foreach (TaskDto taskDto in dto.TasksDtos)
        {
            panel.AddTask(taskDto.ToTask());
        }
        _panelMemory.AddPanel(panel);
    }

    public List<PanelDto?> GetAllPanels()
    {
        List<PanelDto?> panelsDtos = new List<PanelDto?>();
        foreach (Panel panel in _panelMemory.PanelList)
        {
            panelsDtos.Add(PanelDto.FromPanel(panel));
        }
        return panelsDtos;
    }

    public void ModifyPanel(PanelDto? dto)
    {
        ValidatePanelDto(dto);
        Panel panel = _panelMemory.GetPanelById(dto.Id);
        panel.Name = dto.Name!;
        panel.Description = dto.Description!;
    }

    public void DeletePanel(PanelDto? dto, UserDto user, bool permanently)
    {
        Panel panel = _panelMemory.GetPanelById(dto.Id);
        if(permanently)
        {
            _panelMemory.RemovePanelById(panel.Id);
        } 
        else
        {
            RecycleBin recycleBin = _recycleBinMemory.GetRecycleBinByUser(user.ToUser());
            recycleBin.AddPanel(panel);
            panel.Eliminated = true;
            _panelMemory.RemovePanelById(panel.Id);
        }
    }

    public List<PanelDto?>? GetTeamsPanels(List<TeamDto>? teamDtos)
    {
        List<PanelDto?>? panelsDtos = new List<PanelDto?>();
        foreach (TeamDto teamDto in teamDtos)
        {
            List<Panel> panels = _panelMemory.GetTeamPanels(teamDto.Id);
            foreach (Panel panel in panels)
            {
                if (panelsDtos.All(p => p.Id != panel.Id))
                {
                    panelsDtos.Add(PanelDto.FromPanel(panel));
                }
            }
        }
        return panelsDtos;
    }
    
    public PanelDto? GetPanelById(int panelId)
    {
        Panel panel;
        try
        {
            panel = _panelMemory.GetPanelById(panelId);
        }
        catch (Exception e)
        {
            return null;
        }
        
        PanelDto? panelDto = PanelDto.FromPanel(panel);
        return panelDto;
    }
    
    public List<TaskDto> GetPanelTasks(int panelId)
    {
        Panel panel = _panelMemory.GetPanelById(panelId);
        List<TaskDto?> taskDtos = new List<TaskDto?>();
        foreach (Task task in panel.Tasks)
        {
            taskDtos.Add(TaskDto.FromTask(task));
        }
        return taskDtos;
    }
    
    private static void ValidatePanelDto(PanelDto? dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Panel name cannot be empty");
        }
        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new ArgumentException("Panel description cannot be empty");
        }
        if (dto.CreatorDto == null)
        {
            throw new ArgumentException("Panel creator cannot be null");
        }
        if (dto.TeamDto == null)
        {
            throw new ArgumentException("Panel team cannot be null");
        }
    }
}