using Domain;

namespace Memory;

public class PanelMemory
{
    public List<Panel> PanelList { get; } = new();

    public void AddPanel(Panel panel)
    {
        if (PanelList.Contains(panel))
        {
            throw new ArgumentException("Panel already exists");
        }
        PanelList.Add(panel);
    }

    public Panel GetPanelById(int id)
    {
        var panelToGet = PanelList.FirstOrDefault(p => p.Id == id);
        
        if (panelToGet == null)
        {
            throw new ArgumentException();
        }
        return panelToGet;
    }
    
    public void RemovePanelById(int id)
    {
        Panel panelToRemove = PanelList.FirstOrDefault(p => p.Id == id)!;
        if (panelToRemove == null)
        {
            throw new ArgumentException();
        }
        PanelList.Remove(panelToRemove);
    }

    public List<Panel> GetTeamPanels(int id)
    {
        List<Panel> teamPanels = PanelList.Where(p => p.Team.Id == id).ToList();
        
        return teamPanels;
    }
}