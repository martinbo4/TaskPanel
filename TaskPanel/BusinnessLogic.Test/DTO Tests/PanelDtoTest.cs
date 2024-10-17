using BusinessLogic.DTOs;
using Domain;
using Task = Domain.Task;

namespace BusinessLogic.Test.DTO_Tests
{
    [TestClass]
    public class PanelDtoTest
    {
        private User _user;
        private Team _team;
        private Task _task1;
        private Task _task2;

        [TestInitialize]
        public void Setup()
        {
            _user = new User()
            {
                LogIn = new LogIn("example@example.com", "Mateo123."),
            };
            _team = new Team("Team Name", 3)
            {
                Manager = _user,
            };
            _task1 = new Task("task1", "description1");
            _task2 = new Task("task2", "description2");
        }
        
        [TestMethod]
        public void FromPanel_ShouldMapCorrectly()
        {
            Panel panel = new Panel("panel1", "description1", _user, _team)
            {
                Eliminated = false
            };
            panel.AddTask(_task1);
            panel.AddTask(_task2);
            
            PanelDto dto = PanelDto.FromPanel(panel);
            
            Assert.AreEqual(panel.Id, dto.Id);
            Assert.AreEqual(panel.Name, dto.Name);
            Assert.AreEqual(panel.Description, dto.Description);
            Assert.AreEqual(panel.Creator.Id, dto.CreatorDto.Id);
            Assert.AreEqual(panel.Team.Id, dto.TeamDto.Id);
            foreach (TaskDto taskDto in dto.TasksDtos)
            {
                Assert.IsTrue(panel.Tasks.Any(t => t.Id == taskDto.Id));
            }
            Assert.IsFalse(dto.Eliminated);
        }
        
        [TestMethod]
        public void ToPanel_ShouldMapCorrectly()
        {
            PanelDto dto = new PanelDto
            {
                Id = 1,
                Name = "panel1",
                Description = "description1",
                CreatorDto = UserDto.FromUser(_user),
                TeamDto = TeamDto.FromTeam(_team),
                Eliminated = true
            };
            
            Panel panel = dto.ToPanel();
            
            Assert.AreEqual(dto.Id, panel.Id);
            Assert.AreEqual(dto.Name, panel.Name);
            Assert.AreEqual(dto.Description, panel.Description);
            Assert.AreEqual(dto.CreatorDto.Id, panel.Creator.Id);
            Assert.AreEqual(dto.TeamDto.Id, panel.Team.Id);
            foreach (Task task in panel.Tasks)
            {
                Assert.IsTrue(dto.TasksDtos.Any(t => t.Id == task.Id));
            }
            Assert.IsTrue(dto.Eliminated);
        }
    }
}