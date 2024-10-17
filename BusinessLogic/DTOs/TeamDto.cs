using Domain;

namespace BusinessLogic.DTOs;

public class TeamDto
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public UserDto? ManagerDto { get; init; }
    public List<UserDto> MemberDtos { get; set; } = new();
    public int MaxMembers { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; init; }
    public List<PanelDto> PanelsDtos { get; init; } = new();

    public static TeamDto FromTeam(Team team)
    {
        List<UserDto> memberDtos = new List<UserDto>();
        foreach (User member in team.Members)
        {
            memberDtos.Add(UserDto.FromUser(member));
        }
        TeamDto dto = new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            CreationDate = team.CreationDate,
            ManagerDto = UserDto.FromUser(team.Manager!),
            MemberDtos = memberDtos,
            MaxMembers = team.MaxMembers
        };
        foreach (Panel panel in team.Panels)
        {
            dto.PanelsDtos.Add(PanelDto.FromPanel(panel));
        }
        
        return dto;
    }
    
    public Team ToTeam()
    {
        Team team = new Team(Name!, MaxMembers)
        {
            Description = Description!,
            CreationDate = CreationDate,
            Id = Id
        };
        foreach (UserDto memberDto in MemberDtos)
        {
            team.AddUserToTeam(memberDto.ToUser());
        }
        
        return team;
    }
    
    public static TeamDto FromTeam_When_FromPanel(Team team, PanelDto panelDto)
    {
        List<UserDto> memberDtos = new List<UserDto>();
        foreach (User member in team.Members)
        {
            memberDtos.Add(UserDto.FromUser(member));
        }
        TeamDto dto = new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            CreationDate = team.CreationDate,
            ManagerDto = UserDto.FromUser(team.Manager!),
            MemberDtos = memberDtos,
            MaxMembers = team.MaxMembers
        };
        dto.PanelsDtos.Add(panelDto);
        
        return dto;
    }
}