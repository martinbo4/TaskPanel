using Domain;
using Exceptions;

namespace BusinessLogic.Test;

[TestClass]
public class TeamTest
{
    private const string TeamName = "Team1";
    private const int MaxMembers = 1;
    private const string Description = "Description";
    private Team _team;
    
    [TestInitialize]
    public void Setup()
    {
        _team = new Team(TeamName, MaxMembers);
    }
    
    [TestMethod]
    public void CreationOfTeamTest()
    {
        string result = _team.Name;
        
        Assert.AreEqual(TeamName, result);
    }
    
    [TestMethod]
    public void CreationOfTeamWithMaxMembersTest()
    {
        int result = _team.MaxMembers;
        
        Assert.AreEqual(MaxMembers, result);
    }
    
    [TestMethod]
    public void CreationOfTeamWithDescriptionTest()
    {
        Team teamWithDescription = new Team(TeamName, MaxMembers, Description);
        
        string result = teamWithDescription.Description;
        
        Assert.AreEqual(Description, result);
    }
    
    [TestMethod]
    public void TeamIdIsSettingCorrectly()
    {
        Team teamAux = new Team(TeamName, MaxMembers);
        
        int result =teamAux.Id;
        
        Assert.AreEqual(_team.Id+1, result);
    }
    
    [TestMethod]
    public void TeamIsMaxMembersExceedingMinimumTest()
    {
        var result = _team.IsMaxMembersExceedingMinimum();
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void CreationDateTest()
    {
        DateTime result = _team.CreationDate;
        
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void AddUserToTeamTest()
    {
        User user = new User();
        List<User> result = _team.Members;
        
        _team.AddUserToTeam(user);
        
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void AddTwoUserToTeamTest()
    {
        User user1 = new User();
        User user2 = new User();
        
        _team.AddUserToTeam(user1);
        _team.AddUserToTeam(user2);
        
        Assert.AreEqual(user1,_team.Members[0]);
        Assert.AreEqual(user2,_team.Members[1]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(TeamMemberCannotBeNullException))]
    public void AddUserToTeamWithNullUserTest()
    {
        _team.AddUserToTeam(null);
    }

    [TestMethod]
    public void AddOnePanel()
    {
        User user  = new User();
        Panel panel = new Panel("Name", "Description", user, _team);
        
        _team.AddPanel(panel);
        
        Assert.AreEqual(panel, _team.Panels[0]);
    }

    [TestMethod]
    public void FirstUserOnTeamIsManager()
    {
        Team oneTeam = new Team(TeamName, MaxMembers);
        User oneUser = new User();
        
        oneTeam.AddUserToTeam(oneUser);
        
        Assert.AreEqual(oneUser, oneTeam.Manager);
    }
    
    [TestMethod]
    public void SecondUserOnTeamIsNotManager()
    {
        Team oneTeam = new Team(TeamName, MaxMembers);
        User user1 = new User();
        User user2 = new User();
        
        oneTeam.AddUserToTeam(user1);
        oneTeam.AddUserToTeam(user2);
        
        Assert.AreEqual(user1, oneTeam.Manager);
    }
}