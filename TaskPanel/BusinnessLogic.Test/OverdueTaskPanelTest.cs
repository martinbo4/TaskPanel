using BusinessLogic;
using BusinessLogic.DTOs;
using Domain;
using Memory;
using Task = Domain.Task;
using System.Reflection;

namespace BusinnessLogic.Test;

[TestClass]
public class OverdueTaskPanelTest
{
    private TaskLogic _taskLogic;
    private TeamLogic _teamLogic;
    private PanelLogic _panelLogic;
    private UserLogic _userLogic;
    
    private PanelMemory _panelMemory;
    private RecycleBinMemory _recycleBinMemory;
    private TeamMemory _teamMemory;
    private UserMemory _userMemory;
    
    private User _user;
    private Team _team1;
    private Task _task1;
    private Panel _panel1;
    private RecycleBin _recycleBin;

    [TestInitialize]
    public void SetUp()
    {
        _panelMemory = new PanelMemory();
        _recycleBinMemory = new RecycleBinMemory();
        _teamMemory = new TeamMemory();
        _userMemory = new UserMemory();
        _taskLogic = new TaskLogic(_panelMemory, _recycleBinMemory);
        _teamLogic = new TeamLogic(_teamMemory, _userMemory, _panelMemory);
        _panelLogic = new PanelLogic(_panelMemory, _recycleBinMemory, _teamMemory);
        _userLogic = new UserLogic(_userMemory);
        UserDto userDto = new UserDto()
        {
            FirstNameAndLastName = "User1",
            LogInDto = LogInDto.FromLogIn(new LogIn("example@email.com", "Password123.")),
            Birthdate = DateTime.Today.AddDays(-1),
            IsAdmin = false
        };
        _userLogic.CreateUser(userDto);
        _user = _userLogic.GetAllUsers()[0].ToUser();
        _recycleBin = new RecycleBin(_user, 10);
        TeamDto? teamDto1 = new TeamDto()
        {
            Name = "Team1",
            MaxMembers = 10,
            Description = "Description1",
            ManagerDto = UserDto.FromUser(_user),
            MemberDtos = new List<UserDto>(){UserDto.FromUser(_user)},
            PanelsDtos = new List<PanelDto>()
        };
        TeamDto? teamDto2 = new TeamDto()
        {
            Name = "Team2",
            MaxMembers = 10,
            Description = "Description1",
            ManagerDto = UserDto.FromUser(_user),
            MemberDtos = new List<UserDto>(){UserDto.FromUser(_user)},
            PanelsDtos = new List<PanelDto>()
        };
        _teamLogic.CreateTeam(teamDto1);
        _teamLogic.CreateTeam(teamDto2);
        List<TeamDto?> teamDtos = _teamLogic.GetAllTeams();
        _team1 = _teamMemory.TeamList[0];
        PanelDto? panelDto1 = new PanelDto()
        {
            Name = "Panel1",
            Description = "Description1",
            CreatorDto = UserDto.FromUser(_user),
            TeamDto = TeamDto.FromTeam(_team1)
        };
        _panelLogic.CreatePanel(panelDto1);
        _panel1 = _panelMemory.PanelList[2];
        TaskDto? taskDto1 = new TaskDto()
        {
            Title = "Task1",
            Description = "Description1",
            Priority = Priority.Low,
            Deadline = DateTime.Today.AddDays(1),
            PanelDto = PanelDto.FromPanel(_panel1)
        };
        _taskLogic.CreateTask(taskDto1, _panel1.Id);
        _task1 = _panel1.Tasks[0];
        _recycleBinMemory.AddRecycleBin(_recycleBin);
    }
    
    //Funcion para modificar un campo privado de una clase, se usa para setear la fecha de vencimiento de una tarea en el pasado. Se hizo con Microsoft Copilot.
    public static void SetPrivateField(object obj, string fieldName, object value)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
    }

    [TestMethod]
    public void TaskIsDeletedFromOriginalPanel()
    {
        SetPrivateField(_task1, "_deadline", DateTime.Today.AddDays(-1));
        _taskLogic.UpdateTasks();
        
        Assert.IsFalse(_panel1.Tasks.Contains(_task1));
    }
    
    [TestMethod]
    public void TaskIsAddedToOverdueTaskPanel()
    {
        SetPrivateField(_task1, "_deadline", DateTime.Today.AddDays(-1));
        _taskLogic.UpdateTasks();
        
        Assert.IsTrue(_panelMemory.PanelList.Any(p => p.IsOverdueTaskPanel && p.Tasks.Contains(_task1)));
    }
    
    [TestMethod]
    public void TaskIsDeletedFromOverdueTaskPanel_When_ItIsReactivated()
    {
        SetPrivateField(_task1, "_deadline", DateTime.Today.AddDays(-1));
        _taskLogic.UpdateTasks();
        TaskDto? taskDto = TaskDto.FromTask(_task1);
        taskDto.Deadline = DateTime.Today.AddDays(1);
        Panel overdueTaskPanel = _panelMemory.PanelList.Find(p => p.IsOverdueTaskPanel);
        
        _taskLogic.ReactivateTask(taskDto);
        
        Assert.IsFalse(overdueTaskPanel.Tasks.Contains(_task1));
    }
    
    [TestMethod]
    public void TaskIsAddedToOriginalPanel_When_ItIsReactivated()
    {
        SetPrivateField(_task1, "_deadline", DateTime.Today.AddDays(-1));
        _taskLogic.UpdateTasks();
        TaskDto? taskDto = TaskDto.FromTask(_task1);
        taskDto.Deadline = DateTime.Today.AddDays(1);
        
        _taskLogic.ReactivateTask(taskDto);
        
        Assert.IsTrue(_panel1.Tasks.Contains(_task1));
    }
    
    [TestMethod]
    public void OverdueTaskPanelAlwaysExistsForEachTeam()
    {
        
        foreach (Team team in _teamMemory.TeamList)
        {
            Assert.IsTrue(team.Panels.Any(p => p.IsOverdueTaskPanel));
        }
    }
}