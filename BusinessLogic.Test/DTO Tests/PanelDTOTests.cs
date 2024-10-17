using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DTOs;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task = Domain.Task;

namespace BusinessLogic.Test.DTO_Tests
{
    [TestClass]
    public class PanelDTOTests
    {
        [TestMethod]
        public void FromPanel_ShouldMapCorrectly()
        {
            var user = new User();
            var tasks = new List<Task>();
            Task task1 = new Task("task1", "description1");
            Task task2 = new Task("task2", "description2");
            var panel = new Panel("panel1", "description1", user)
            {
                TeamId = 1,
                Eliminated = false
            };
            panel.Tasks.Add(task1);
            panel.Tasks.Add(task2);
            
            var dto = PanelDTO.FromPanel(panel);
            
            Assert.AreEqual(panel.Id, dto.Id);
            Assert.AreEqual(panel.Name, dto.Name);
            Assert.AreEqual(panel.Description, dto.Description);
            Assert.AreEqual(panel.Creator.Id, dto.CreatorId);
            Assert.AreEqual(panel.TeamId, dto.TeamId);
            foreach (var task in dto.TaskIds)
            {
                Assert.IsTrue(panel.Tasks.Select(t => t.Id).ToList().Contains(task));
            }
            Assert.IsFalse(dto.Eliminated);
        }
        
        [TestMethod]
        public void ToPanel_ShouldMapCorrectly()
        {
            var user = new User ();
            Task task1 = new Task("task1", "description1");
            Task task2 = new Task("task2", "description2");
            var tasks = new List<Task> { task1, task2 };
            var dto = new PanelDTO
            {
                Id = 1,
                Name = "panel1",
                Description = "description1",
                CreatorId = user.Id,
                TeamId = 1,
                TaskIds = new List<int> { task1.Id, task2.Id },
                Eliminated = true
            };
            var panel = dto.ToPanel(user, tasks);
            
            Assert.AreEqual(dto.Id, panel.Id);
            Assert.AreEqual(dto.Name, panel.Name);
            Assert.AreEqual(dto.Description, panel.Description);
            Assert.AreEqual(dto.CreatorId, panel.Creator.Id);
            Assert.AreEqual(dto.TeamId, panel.TeamId);
            foreach (var task in panel.Tasks)
            {
                Assert.IsTrue(dto.TaskIds.Contains(task.Id));
            }
            Assert.IsTrue(dto.Eliminated);
        }
    }
}