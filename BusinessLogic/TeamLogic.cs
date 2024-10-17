using BusinessLogic.DTOs;
using Domain;
using Memory;

namespace BusinessLogic;

public class TeamLogic
{
    private readonly TeamMemory _teamMemory;
    private readonly UserMemory _userMemory;
    private readonly PanelMemory _panelMemory;

    public TeamLogic(TeamMemory teamMemory, UserMemory userMemory, PanelMemory panelMemory)
    {
        _teamMemory = teamMemory;
        _userMemory = userMemory;
        _panelMemory = panelMemory;
    }
    
    public void CreateTeam(TeamDto teamDto)
    {
        ValidateTeamDto(teamDto);
        Team team = new Team(teamDto.Name!, teamDto.MaxMembers, teamDto.Description!);
        foreach (UserDto userDto in teamDto.MemberDtos)
        {
            team.AddUserToTeam(userDto.ToUser());
        }
        foreach (PanelDto panelDto in teamDto.PanelsDtos)
        {
            team.AddPanel(panelDto.ToPanel());
        }
        _teamMemory.AddTeam(team);
        Panel overdueTasksPanel = CreateOverdueTaskPanel(team);
        team.AddPanel(overdueTasksPanel);
        _panelMemory.AddPanel(overdueTasksPanel);
    }
    
    public List<TeamDto> GetAllTeams()
    {
        List<TeamDto> teams = new List<TeamDto>();
        foreach (Team team in _teamMemory.TeamList)
        {
            teams.Add(TeamDto.FromTeam(team));
        }
        return teams;
    }

    public void DeleteTeamById(int teamId)
    {
        foreach (Panel panel in _panelMemory.PanelList)
        {
            if(panel.Team.Id == teamId)
            {
                throw new ArgumentException("Cannot delete team with active panels");
            }
        }
        _teamMemory.RemoveTeamById(teamId);
    }

    public void ModifyTeam(TeamDto teamDto)
    {
        ValidateTeamDto(teamDto);
        Team team = _teamMemory.GetTeamById(teamDto.Id);
        team.Name = teamDto.Name!;
        team.Description = teamDto.Description!;
        team.MaxMembers = teamDto.MaxMembers;
    }
    
    public void AddMemberToTeam(int teamId, string userEmail)
    {
        Team team = _teamMemory.GetTeamById(teamId);
        User user = _userMemory.GetUserByEmail(userEmail);
        VerifyTeamIsFull(team);
        VerifyIfUserIsInTeam(team, user);
        team.Members.Add(user);
    }
    
    private static void VerifyTeamIsFull(Team team)
    {
        if(team.Members.Count >= team.MaxMembers)
        {
            throw new ArgumentException("Team is full");
        }
    }
    
    private static void VerifyIfUserIsInTeam(Team team, User user)
    {
        foreach(User member in team.Members)
        {
            if(member.Id == user.Id)
            {
                throw new ArgumentException("User is already in the team");
            }
        }
    }

    public void RemoveMemberFromTeam(int teamId, string userEmail)
    {
        Team team = _teamMemory.GetTeamById(teamId);
        User user = _userMemory.GetUserByEmail(userEmail);
        if (team.Members.Count == 1)
        {
            throw new ArgumentException("Cannot remove the only member of the team");
        }
        team.RemoveUserFromTeamById(user.Id);
        team.Manager = team.Members[0];
    }
    
    public TeamDto GetTeamById(int teamId)
    {
        Team team;
        try 
        {
            team = _teamMemory.GetTeamById(teamId);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Must provide a valid team");
        }
        return TeamDto.FromTeam(team);
    }

    public List<TeamDto> GetUserTeams(UserDto user)
    {
        List<TeamDto> userTeamsDtos = new List<TeamDto>();
        List<Team> userTeams = _teamMemory.GetUserTeamsById(user.Id);
        foreach (Team team in userTeams)
        {
            userTeamsDtos.Add(TeamDto.FromTeam(team));
        }
        return userTeamsDtos;
    }
    
    private static void ValidateTeamDto(TeamDto teamDto)
    {
        if (teamDto.MaxMembers < 1)
        {
            throw new ArgumentException("Max members must be at least 1");
        }
        if (teamDto.MemberDtos.Count < 1)            
        {
            throw new ArgumentException("Team must have at least one member");
        }
        if (string.IsNullOrEmpty(teamDto.Name))
        {
            throw new ArgumentException("Team name cannot be empty");
        }
        if (string.IsNullOrEmpty(teamDto.Description))
        {
            throw new ArgumentException("Team description cannot be empty");
        }
        if (teamDto.MaxMembers < teamDto.MemberDtos.Count)
        {
            throw new ArgumentException("Max members cannot be less than the number of members");
        }
    }

    private Panel CreateOverdueTaskPanel(Team team)
    {
        string description = "This panel contains all the overdue tasks from "+team.Name+" team.";
        Panel overdueTasksPanel = new Panel("Overdue Tasks Panel from "+team.Name+" team.", description, team.Manager!, team)
        {
            IsOverdueTaskPanel = true
        };
        return overdueTasksPanel;
    }
}