using BusinessLogic.DTOs;
using Domain;
using Memory;

namespace BusinessLogic;

public class RecycleBinLogic
{
    private readonly RecycleBinMemory _recycleBinMemory;
    private readonly UserMemory _userMemory;
    
    public RecycleBinLogic(RecycleBinMemory recycleBinMemory, UserMemory userMemory)
    {
        _recycleBinMemory = recycleBinMemory;
        _userMemory = userMemory;
    }

    public RecycleBinDto GetRecycleBin(int userId)
    {
        return RecycleBinDto.FromRecycleBin(_recycleBinMemory.GetRecycleBinByUser(_userMemory.GetUserById(userId)));
    }
    
    public void EmptyRecycleBin(int userId)
    {
        RecycleBin recycleBin = _recycleBinMemory.GetRecycleBinByUser(_userMemory.GetUserById(userId));
        recycleBin.Panels.Clear();
        recycleBin.Tasks.Clear();
    }

    public void RemoveTask(TaskDto? taskDto, int userId)
    {
        RecycleBin recycleBin = _recycleBinMemory.GetRecycleBinByUser(_userMemory.GetUserById(userId));
        recycleBin.RemoveTask(taskDto.Id);
    }
    
    public void RemovePanel(PanelDto? panelDto, int userId)
    {
        RecycleBin recycleBin = _recycleBinMemory.GetRecycleBinByUser(_userMemory.GetUserById(userId));
        recycleBin.RemovePanel(panelDto.Id);
    }
}