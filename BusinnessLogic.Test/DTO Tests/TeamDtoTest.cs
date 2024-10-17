using BusinessLogic.DTOs;
using Domain;

namespace BusinessLogic.Test.DTO_Tests;

[TestClass]
public class TeamDtoTest
{
    [TestMethod]
    public void FromTeam_ShouldMapCorrectly()
    {
        User user1 = new User()
        {
            LogIn = new LogIn("user1@gmail.com", "Mateo123."),
        };
        User user2 = new User()
        {
            LogIn = new LogIn("user2@gmail.com", "Mateo123."),
        };
        Team team = new Team("team1", 5);
        Panel panel = new Panel("Name", "Description", user1, team);
        team.AddUserToTeam(user1);
        team.AddUserToTeam(user2);
        team.AddPanel(panel);

        TeamDto dto = TeamDto.FromTeam(team);

        Assert.AreEqual(team.Id, dto.Id);
        Assert.AreEqual(team.Name, dto.Name);
        Assert.AreEqual(team.Description, dto.Description);
        Assert.AreEqual(team.CreationDate, dto.CreationDate);
        Assert.AreEqual(team.Manager.Id, dto.ManagerDto.Id);
        foreach (User member in team.Members)
        {
            Assert.IsTrue(dto.MemberDtos.Any(m => m.Id == member.Id));
        }
        foreach (Panel teamPanel in team.Panels)
        {
                Assert.IsTrue(dto.PanelsDtos.Any(p => p.Id == teamPanel.Id));
        }
        Assert.AreEqual(team.MaxMembers, dto.MaxMembers);
    }
        
    [TestMethod]
    public void ToTeam_ShouldMapCorrectly()
    {
        User oneUser = new User()
        {
            LogIn = new LogIn("user1@gmail.com", "Mateo123."),
        };
        List<UserDto> users = new List<UserDto>();
        users.Add(UserDto.FromUser(oneUser));
        TeamDto dto = new TeamDto()
        {
            Id = 1,
            Name = "team1",
            Description = "team1 description",
            CreationDate = DateTime.Now,
            ManagerDto = UserDto.FromUser(oneUser),
            MemberDtos = users,
            MaxMembers = 3
        };
            
        Team teamMapped = dto.ToTeam();
            
        Assert.AreEqual(dto.Id, teamMapped.Id);
        Assert.AreEqual(dto.Name, teamMapped.Name);
        Assert.AreEqual(dto.ManagerDto.Id, teamMapped.Manager.Id);
        foreach (User member in teamMapped.Members)
        {
            Assert.IsTrue(dto.MemberDtos.Any(m => m.Id == member.Id));
        }
        Assert.AreEqual(dto.MaxMembers, teamMapped.MaxMembers);
        Assert.AreEqual(dto.CreationDate, teamMapped.CreationDate);
        Assert.AreEqual(dto.Description, teamMapped.Description);
    }
}    