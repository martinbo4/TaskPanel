using Domain;

namespace Memory.Test;

[TestClass]
public class TeamMemoryTest
{
    private TeamMemory _teamMemory;
    private Team _team;
    
    [TestInitialize]
    public void SetUp()
    {
        _teamMemory = new TeamMemory();
        _team = new Team("Team1", 2);
    }
    [TestMethod]
    public void TeamListIsNotNullAfterConstructor()
    {
        Assert.IsNotNull(_teamMemory.TeamList);
    }
    
    [TestMethod]
    public void AddOneTeam()
    {
        _teamMemory.AddTeam(_team);
        
        Assert.AreEqual(_team, _teamMemory.TeamList[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsExceptionWhenTeamAttemptedToDeleteDoesntExists()
    {
        _teamMemory.RemoveTeamById(_team.Id);
    }
    
    [TestMethod]
    public void GetOneTeamById()
    {
        _teamMemory.AddTeam(_team);
        
        Team team = _teamMemory.GetTeamById(_team.Id);
        
        Assert.AreEqual(_team, team);
    }

    [TestMethod]
    public void GetUserTeamsTest()
    {
        User user = new();
        Team team = new Team("Team2", 2);
        _teamMemory.AddTeam(team);
        _teamMemory.AddTeam(_team);
        _team.AddUserToTeam(user);
        team.AddUserToTeam(user);
        
        List<Team> userTeams = _teamMemory.GetUserTeamsById(user.Id);
        
        Assert.IsTrue(userTeams.Contains(team));
        Assert.IsTrue(userTeams.Contains(_team));
    }
}