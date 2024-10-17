using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using Memory;

namespace BusinnessLogic.Test;

[TestClass]
public class TeamLogicTest
{
    private TeamMemory _teamMemory;
    private UserMemory _userMemory;
    private PanelMemory _panelMemory;
    private User _user1;
    private User _user2;
    private Team _team1 = new Team("Team 1",2, "Description");
    private Team _team2 = new Team("Team 2",2, "Description");
    private TeamLogic _teamLogic;

    [TestInitialize]
    public void Setup()
    {
        _teamMemory = new TeamMemory();
        _panelMemory = new PanelMemory();
        _userMemory = new UserMemory();
        _teamMemory.AddTeam(_team1);
        _teamMemory.AddTeam(_team2);
        _user1 = new User()
        {
            LogIn = new LogIn("user1@gmail.com", "Mateo123."),
        };
        _user2 = new User()
        {
            LogIn = new LogIn("user2@gmail.com", "Mateo123."),
        };
        _teamLogic = new TeamLogic(_teamMemory, _userMemory, _panelMemory);
    }
    
    [TestMethod]
    public void GetAllTeamsTest()
    {
        _team1.AddUserToTeam(_user1);
        _team2.AddUserToTeam(_user2);
        TeamDto teamDTO1 = TeamDto.FromTeam(_team1);
        TeamDto teamDTO2 = TeamDto.FromTeam(_team2);
        
        _teamLogic.CreateTeam(teamDTO1);
        _teamLogic.CreateTeam(teamDTO2);

        List<TeamDto> teams = _teamLogic.GetAllTeams();
        
        Assert.IsTrue(teams.Any(t=>t.Name=="Team 1"));
        Assert.IsTrue(teams.Any(t=>t.Name=="Team 2"));
    }
    
    [TestMethod]
    public void DeleteOneTeam()
    {
        _team1.AddUserToTeam(_user1);
        _team2.AddUserToTeam(_user2);
        
        TeamLogic teamLogic = new TeamLogic(_teamMemory, _userMemory, _panelMemory);
        
        teamLogic.DeleteTeamById(_team1.Id);
        
        List<TeamDto> teams = teamLogic.GetAllTeams();
        Assert.IsFalse(teams.Any(t => t.Id == _team1.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsException_When_TeamHasPanels()
    {
        Panel panel = new Panel("Name", "Description", _user1, _team1);
        _panelMemory.AddPanel(panel);
        
        _teamLogic.DeleteTeamById(_team1.Id);
    }
    
    [TestMethod]
    public void ModifyOneTeam()
    {
        _team1.AddUserToTeam(_user1);
        _team2.AddUserToTeam(_user2);
        string newName = "New Name";
        string newDescription = "New Description";
        int newMaxMembers = 3;
        
        TeamDto teamDto = new TeamDto
        {
            Id = _team1.Id,
            Name = newName,
            Description = newDescription,
            MaxMembers = newMaxMembers,
            MemberDtos = _team1.Members.Select(UserDto.FromUser).ToList()
        };
        
        _teamLogic.ModifyTeam(teamDto);
        
        List<TeamDto> teams = _teamLogic.GetAllTeams();
        Assert.IsTrue(teams.Any(t => t.Name == newName));
        Assert.IsTrue(teams.Any(t => t.Description == newDescription));
        Assert.IsTrue(teams.Any(t => t.MaxMembers == newMaxMembers));
    }

    [TestMethod]
    public void AddMemberToTeamAddsUserToTeamWhenUserAndTeamExist()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        _userMemory.AddUser(user);
        _teamLogic.AddMemberToTeam(_team1.Id, user.LogIn.Email);
    }

    [TestMethod]
    public void AddMemberToTeamThrowsExceptionWhenTeamDoesNotExist()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        _userMemory.AddUser(user);
        
        Assert.ThrowsException<ArgumentException>(() => _teamLogic.AddMemberToTeam(0, user.LogIn.Email));
    }

    [TestMethod]
    public void AddMemberToTeamThrowsExceptionWhenUserDoesNotExist()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        
        Assert.ThrowsException<ArgumentException>(() => _teamLogic.AddMemberToTeam(_team1.Id, user.LogIn.Email));
    }

    [TestMethod]
    public void AddMemberToTeamThrowsExceptionWhenUserIsAlreadyInTeam()
    {
        _userMemory.AddUser(_user1);
        _team1.AddUserToTeam(_user1);
        
        Assert.ThrowsException<ArgumentException>(() => _teamLogic.AddMemberToTeam(_team1.Id, _user1.LogIn.Email));
    }
    
    [TestMethod]
    public void AddMemberToTeamThrowsExceptionWhenTeamIsFull()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        _userMemory.AddUser(_user1);
        _userMemory.AddUser(_user2);
        _team1.AddUserToTeam(_user1);
        _team1.AddUserToTeam(_user2);
        _userMemory.AddUser(user);
        
        Assert.ThrowsException<ArgumentException>(() => _teamLogic.AddMemberToTeam(_team1.Id, user.LogIn.Email));
    }

    [TestMethod]
    public void RemoveMemberFromTeamRemovesUserFromTeamWhenUserAndTeamExist()
    {
        LogIn logIn1 = new LogIn("example1@email.com", "Mateo123346.");
        LogIn logIn2 = new LogIn("example2@email.com", "Mateo123346.");
        User user1 = new User(logIn1, "Jane Doe", new DateTime(2004, 03, 10), false);
        User user2 = new User(logIn2, "Jane Doe", new DateTime(2004, 03, 10), false);
        _userMemory.AddUser(user1);
        _userMemory.AddUser(user2);
        _teamLogic.AddMemberToTeam(_team1.Id, user1.LogIn.Email);
        _teamLogic.AddMemberToTeam(_team1.Id, user2.LogIn.Email);
        _teamLogic.RemoveMemberFromTeam(_team1.Id, user2.LogIn.Email);

        Assert.IsFalse(_team1.Members.Contains(user2));
    }

    [TestMethod]
    public void RemoveMemberFromTeamThrowsExceptionWhenTeamDoesNotExist()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);
        _userMemory.AddUser(user);

        Assert.ThrowsException<ArgumentException>(() => _teamLogic.RemoveMemberFromTeam(0, user.LogIn.Email));
    }

    [TestMethod]
    public void RemoveMemberFromTeamThrowsExceptionWhenUserDoesNotExist()
    {
        LogIn logIn = new LogIn("example1@email.com", "Mateo123346.");
        User user = new User(logIn, "Jane Doe", new DateTime(2004, 03, 10), false);

        Assert.ThrowsException<ArgumentException>(() => _teamLogic.RemoveMemberFromTeam(_team1.Id, user.LogIn.Email));
    }
    
    [TestMethod]
    public void GetTeamByIdTest()
    {
        _team1.AddUserToTeam(_user1);
        TeamDto team = _teamLogic.GetTeamById(_team1.Id);
        Team result = team.ToTeam();
        
        Assert.AreEqual(_team1.Id, result.Id);
        Assert.AreEqual(_team1.Name, result.Name);
        Assert.AreEqual(_team1.Description, result.Description);
        Assert.AreEqual(_team1.CreationDate, result.CreationDate);
        Assert.AreEqual(_team1.Manager.Id, result.Manager.Id);
        Assert.AreEqual(_team1.MaxMembers, result.MaxMembers);
    }
    
    [TestMethod]
    public void GetTeamByIdThrowsExceptionWhenTeamDoesNotExist()
    {
        
        Assert.ThrowsException<ArgumentException>(() => _teamLogic.GetTeamById(0));
    }

    [TestMethod]
    public void GetUserTeamsTest()
    {
        _user1 = new User()
        {
            LogIn = new LogIn("example1@email.com", "Mateo123346."),
        };
        _team1.AddUserToTeam(_user1);
        _team2.AddUserToTeam(_user1);
        
        List<TeamDto> userTeamsDtos = _teamLogic.GetUserTeams(UserDto.FromUser(_user1));
        
        Assert.IsTrue(userTeamsDtos.Any(t => t.Id == _team1.Id));
        Assert.IsTrue(userTeamsDtos.Any(t => t.Id == _team2.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Max members must be at least 1")]
    public void CreateTeam_MaxMembersLessThanOne_ThrowsArgumentException()
    {
        TeamDto teamDto = new TeamDto { MaxMembers = 0, MemberDtos = new List<UserDto> { new UserDto() }, Name = "Team", Description = "Description" };
        _teamLogic.CreateTeam(teamDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Team must have at least one member")]
    public void CreateTeam_NoMembers_ThrowsArgumentException()
    {
        TeamDto teamDto = new TeamDto { MaxMembers = 5, MemberDtos = new List<UserDto>(), Name = "Team", Description = "Description" };
        _teamLogic.CreateTeam(teamDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Team name cannot be empty")]
    public void CreateTeam_EmptyName_ThrowsArgumentException()
    {
        TeamDto teamDto = new TeamDto { MaxMembers = 5, MemberDtos = new List<UserDto> { new UserDto() }, Name = "", Description = "Description" };
        _teamLogic.CreateTeam(teamDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Team description cannot be empty")]
    public void CreateTeam_EmptyDescription_ThrowsArgumentException()
    {
        TeamDto teamDto = new TeamDto { MaxMembers = 5, MemberDtos = new List<UserDto> { new UserDto() }, Name = "Team", Description = "" };
        _teamLogic.CreateTeam(teamDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Max members cannot be less than the number of members")]
    public void CreateTeam_MaxMembersLessThanMembersCount_ThrowsArgumentException()
    {
        TeamDto teamDto = new TeamDto { MaxMembers = 1, MemberDtos = new List<UserDto> { new UserDto(), new UserDto() }, Name = "Team", Description = "Description" };
        _teamLogic.CreateTeam(teamDto);
    }
}