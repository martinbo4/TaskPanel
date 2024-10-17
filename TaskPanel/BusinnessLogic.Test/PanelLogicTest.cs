using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using Memory;
using Task = Domain.Task;

namespace BusinnessLogic.Test;

[TestClass]
public class PanelLogicTest
{
    private PanelLogic _panelLogic;
    private PanelMemory _panelMemory;
    private RecycleBinMemory _recycleBinMemory;
    private TeamMemory _teamMemory;
    private RecycleBin _recycleBin;
    private User _user;
    private Panel _panel1;
    private Task _task1;
    private Team _team1;
    
    [TestInitialize]
    public void SetUp()
    {
        _panelMemory = new PanelMemory();
        _recycleBinMemory = new RecycleBinMemory();
        
        _user = new User()
        {
            LogIn = new LogIn("example@example.com", "Mateo123."),
        };
        _team1 = new Team("Team1", 3);
        _team1.AddUserToTeam(_user);
        _panel1 = new Panel("Panel1", "Description1", _user, _team1);
        _teamMemory = new TeamMemory();
        _teamMemory.AddTeam(_team1);
        _task1 = new Task("Task1", "Description1")
        {
            Panel = _panel1
        };
        _panel1.AddTask(_task1);
        _panelLogic = new PanelLogic(_panelMemory, _recycleBinMemory, _teamMemory);
        _recycleBin = new RecycleBin(_user, 10);
        _recycleBinMemory.AddRecycleBin(_recycleBin);
    }

    [TestMethod]
    public void CreatePanelTest()
    {
        PanelDto? dto = new PanelDto()
        {
            Name = "Name",
            Description = "Description",
            CreatorDto = new UserDto()
            {
                LogInDto = new LogInDto()
                {
                    Email = "email@email.com", Password = "Mateo123."
                }
            },
            TeamDto = TeamDto.FromTeam(_team1),
            Eliminated = false
        };

        _panelLogic.CreatePanel(dto);
        List<PanelDto?> panelsDtos = _panelLogic.GetAllPanels();
        
        Assert.IsTrue(panelsDtos.Any(p => p.Name == "Name"));
    }

    [TestMethod]
    public void ModifyPanelTest()
    {
        UserDto oneUser = new UserDto()
        {
            LogInDto = new LogInDto()
            {
                Email = "email@email.com", Password = "Mateo123."
            }
        };
        PanelDto? oldDto = new PanelDto()
        {
            Name = "Name",
            Description = "Description",
            CreatorDto = oneUser,
            TeamDto = TeamDto.FromTeam(_team1),
            Eliminated = false
        };
        
        _panelLogic.CreatePanel(oldDto);
        List<PanelDto?> oldList = _panelLogic.GetAllPanels();
        PanelDto? newDto = oldList[0];
        newDto.Name = "Name2";
        newDto.Description = "Description2";
        
        _panelLogic.ModifyPanel(newDto);
        List<PanelDto?> newList = _panelLogic.GetAllPanels();
        PanelDto? actualDto = newList[0];
        
        Assert.AreEqual(newDto.Name, actualDto.Name);
        Assert.AreEqual(newDto.Description, actualDto.Description);
    }
    
    [TestMethod]
    public void DeletePanelTest()
    {
        _panelMemory.AddPanel(_panel1);
        PanelDto? panelDto = _panelLogic.GetAllPanels()[0];
        UserDto userDto = panelDto.CreatorDto;
        
        _panelLogic.DeletePanel(panelDto, userDto, false);
        
        Assert.IsTrue(_recycleBin.Panels.Any(p => p.Id == panelDto.Id));
        Assert.IsTrue(_panelMemory.PanelList.Count == 0);
    }
    
    [TestMethod]
    public void GetTeamsPanels_ReturnsCorrectPanels()
    {
        List<TeamDto?> teamDtos = new List<TeamDto?>
        {
            TeamDto.FromTeam(_team1),
        };
        Panel panel1 = new Panel("Panel1", "Description1", _team1.Manager, _team1);
        Panel panel2 = new Panel("Panel2", "Description2", _team1.Manager, _team1);
        _panelMemory.AddPanel(panel1);
        _panelMemory.AddPanel(panel2);

        List<PanelDto?>? result = _panelLogic.GetTeamsPanels(teamDtos);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Panel1", result[0].Name);
        Assert.AreEqual("Panel2", result[1].Name);
    }
    
    [TestMethod]
    public void GetPanelById_ReturnsCorrectPanel()
    {
        Panel panel = new Panel("Panel1", "Description1", _team1.Manager, _team1);
        _panelMemory.AddPanel(panel);

        PanelDto? result = _panelLogic.GetPanelById(panel.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(panel.Id, result.Id);
        Assert.AreEqual(panel.Name, result.Name);
        Assert.AreEqual(panel.Description, result.Description);
    }
    
    [TestMethod]
    public void GetPanelTasks_ReturnsCorrectTasks()
    {
        Panel panel = new Panel("Panel1", "Description1", _team1.Manager, _team1);
        Task task1 = new Task("Task1", "Description1");
        Task task2 = new Task("Task2", "Description2");
        panel.AddTask(task1);
        panel.AddTask(task2);
        _panelMemory.AddPanel(panel);

        List<TaskDto?> result = _panelLogic.GetPanelTasks(panel.Id);

        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Any(t => t.Title == "Task1"));
        Assert.IsTrue(result.Any(t => t.Title == "Task2"));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Panel name cannot be empty")]
    public void CreatePanel_EmptyName_ThrowsArgumentException()
    {
        PanelDto? dto = new PanelDto { Name = "", Description = "Description", CreatorDto = UserDto.FromUser(_user), TeamDto = new TeamDto() };
        _panelLogic.CreatePanel(dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Panel description cannot be empty")]
    public void CreatePanel_EmptyDescription_ThrowsArgumentException()
    {
        PanelDto? dto = new PanelDto { Name = "Panel", Description = "", CreatorDto = UserDto.FromUser(_user), TeamDto = new TeamDto() };
        _panelLogic.CreatePanel(dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Panel creator cannot be null")]
    public void CreatePanel_NullCreator_ThrowsArgumentException()
    {
        PanelDto? dto = new PanelDto { Name = "Panel", Description = "Description", CreatorDto = null, TeamDto = new TeamDto() };
        _panelLogic.CreatePanel(dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Panel team cannot be null")]
    public void CreatePanel_NullTeam_ThrowsArgumentException()
    {
        PanelDto? dto = new PanelDto { Name = "Panel", Description = "Description", CreatorDto = UserDto.FromUser(_user), TeamDto = null };
        _panelLogic.CreatePanel(dto);
    }

    [TestMethod]
    public void CreatePanel_ValidDto_DoesNotThrowException()
    {
        PanelDto? dto = new PanelDto
        {
            Name = "Panel", 
            Description = "Description", 
            CreatorDto = UserDto.FromUser(_user), 
            TeamDto = TeamDto.FromTeam(_team1)
        };
        _panelLogic.CreatePanel(dto);
    }
    
    
}