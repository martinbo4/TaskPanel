using BusinessLogic.DTOs;
using Domain;
using Task = Domain.Task;

namespace BusinnessLogic.Test.DTO_Tests
{
    [TestClass]
    public class RecycleBinDtoTest
    {
        private User _user;
        private List<Panel> _panels;
        private List<Task> _tasks;
        private Panel _panel1;
        private Panel _panel2;
        private Task _task1;
        private Task _task2;
        private Team _team;
        
        [TestInitialize]
        public void setUp()
        {
            LogIn logIn = new LogIn("mateo@gmail.com", "Mateo123.");
            _user = new User(logIn, "Mateo Garcia", new DateTime(2000, 10, 10), false);
            _team = new Team("team1", 2);
            _team.AddUserToTeam(_user);
            _panels = new List<Panel>();
            _panel1 = new Panel("panel1", "description1", _user, _team);
            _panel2 = new Panel("panel2", "description2", _user, _team);
            _team.AddPanel(_panel1);
            _team.AddPanel(_panel2);
            _panels.Add(_panel1);
            _tasks = new List<Task>();
            _task1 = new Task("task1", "description1");
            _task2 = new Task("task2", "description2");
            _panel1.AddTask(_task1);
            _panel2.AddTask(_task2);
            _tasks.Add(_task2);
        }
        
        [TestMethod]
        public void FromRecycleBin_ShouldMapCorrectly()
        {
            RecycleBin recycleBin = new RecycleBin(_user, 10)
            {
                Panels = _panels,
                Tasks = _tasks
            };
            
            RecycleBinDto dto = RecycleBinDto.FromRecycleBin(recycleBin);

            Assert.AreEqual(_user.Id, dto.UserDto.Id);
            foreach (PanelDto panelDto in dto.PanelsDtos)
            {
                Assert.IsTrue(_panels.Any(p => p.Id == panelDto.Id));    
            }
            foreach (TaskDto taskDto in dto.TasksDtos)
            {
                Assert.IsTrue(_tasks.Any(t => t.Id == taskDto.Id));
            }
            Assert.AreEqual(10, dto.MaxCapacity);
        }

        [TestMethod]
        public void ToRecycleBin_ShouldMapCorrectly()
        {
            UserDto userDto = UserDto.FromUser(_user);
            TaskDto taskDto = TaskDto.FromTask(_task1);
            PanelDto panelDto = PanelDto.FromPanel(_panel1);
            RecycleBinDto dto = new RecycleBinDto
            {
                UserDto = userDto,
                PanelsDtos = new List<PanelDto>() { panelDto },
                TasksDtos = new List<TaskDto>() { taskDto },
                MaxCapacity = 10
            };
            
            RecycleBin recycleBin = dto.ToRecycleBin();
            
            Assert.AreEqual(recycleBin.User.Id, dto.UserDto.Id);
            foreach (PanelDto? panelDto1 in dto.PanelsDtos)
            {
                Assert.IsTrue(recycleBin.Panels.Any(p => p.Id == panelDto1.Id));
            }
            foreach (TaskDto? taskDto1 in dto.TasksDtos)
            {
                Assert.IsTrue(recycleBin.Tasks.Any(t => t.Id == taskDto1.Id));
            }
            Assert.AreEqual(10, recycleBin.MaxCapacity);
        }
    }
}