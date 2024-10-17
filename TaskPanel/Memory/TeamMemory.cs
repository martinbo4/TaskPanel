using Domain;

namespace Memory;

public class TeamMemory
{
    public List<Team> TeamList { get; } = new();
    
    public void AddTeam(Team team)
    {
        if (TeamList.Contains(team))
        {
            throw new ArgumentException();
        }
        TeamList.Add(team);
    }
    
    public void RemoveTeamById(int id)
    {
        Team teamToRemove = TeamList.FirstOrDefault(t => t.Id == id)!;
        if (teamToRemove == null)
        {
            throw new ArgumentException();
        }
        TeamList.Remove(teamToRemove);
    }
    
    public Team GetTeamById(int id)
    {
        Team teamToGet = TeamList.FirstOrDefault(t => t.Id == id)!;
        if (teamToGet == null)
        {
            throw new ArgumentException();
        }
        return teamToGet;
    }
    
    public List<Team> GetUserTeamsById(int id)
    {
        List<Team> userTeams = TeamList.Where(t => t.Members.Any(m => m.Id == id)).ToList();
        return userTeams;
    }
}