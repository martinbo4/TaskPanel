namespace Memory.Test;

using Domain;

[TestClass]
public class PanelMemoryTest
{
    private PanelMemory _panelMemory;
    private Task _task;
    private Panel _panel;
    private User _user;
    
    [TestInitialize]
    public void Setup()
    {
        _panelMemory = new PanelMemory();
        _task = new Task("Task Name", "Description");
        _panel = new Panel("Name", "Description", _user, new Team("Any team", 3));
        _panel.AddTask(_task);
    }
    
    [TestMethod]
    public void PanelListIsNotNullAfterConstructor()
    {
        Assert.IsNotNull(_panelMemory.PanelList);
    }

    [TestMethod]
    public void AddOnePanel()
    {
        _panelMemory.AddPanel(_panel);
        
        Assert.AreEqual(_panelMemory.PanelList[0], _panel);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsExceptionWhenPanelAttemptedToAddAlreadyExists()
    {
        _panelMemory.AddPanel(_panel);
        _panelMemory.AddPanel(_panel);
    }
    
    [TestMethod]
    public void GetOnePanelById()
    {
        _panelMemory.AddPanel(_panel);
        
        Panel panel = _panelMemory.GetPanelById(_panel.Id);
        
        Assert.AreEqual(_panel, panel);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsExceptionWhenPanelAttemptedToDeleteDoesntExists()
    {
        _panelMemory.RemovePanelById(_panel.Id);
    }

    [TestMethod]
    public void GetPanelsByTeamId()
    {
        Team team = new Team("Team 1", 2);
        Panel panel = new Panel("Name", "Description", _user, team);
        _panel.Team = team;
        
        _panelMemory.AddPanel(panel);
        _panelMemory.AddPanel(_panel);
        
        List<Panel> panelList = _panelMemory.GetTeamPanels(team.Id);
        bool containsBoth = panelList.Contains(panel) && panelList.Contains(_panel);
        
        Assert.IsTrue(containsBoth);
    }
}