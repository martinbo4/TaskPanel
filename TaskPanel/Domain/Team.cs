using Exceptions;

namespace Domain;

public class Team
{
    private const int MinMembers = 1;
    private static int _idCounter;
    public int Id { get; init; }
    public string Name { get; set; }
    public int MaxMembers { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; init; }
    public User? Manager { get; set; }
    public List<User> Members { get;} = new();
    public List<Panel> Panels { get; } = new();
    
    public Team(string teamName, int maxMembers, string description = "")
    {
        Name = teamName;
        MaxMembers = maxMembers;
        Description = description;
        CreationDate = DateTime.Now;
        Id = ++_idCounter;
    }

    public bool IsMaxMembersExceedingMinimum()
    {
        return MaxMembers >= MinMembers;
    }
    
    public void AddUserToTeam(User user)
    {
        if(UserIsNull(user)) throw new TeamMemberCannotBeNullException("Member cannot be null");
        if (Members.Count == 0)
        {
            Manager = user;
        }
        Members.Add(user);
    }
    
    public void RemoveUserFromTeamById(int userId)
    {
        User user = Members.FirstOrDefault(u => u.Id == userId);
        Members.Remove(user);
    }
    
    private static bool UserIsNull(User user)
    {
        return user == null;
    }

    public void AddPanel(Panel panel)
    {
        Panels.Add(panel);
    }
}