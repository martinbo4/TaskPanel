using Domain;

namespace Memory;

public class RecycleBinMemory
{
    public List<RecycleBin> RecycleBins { get; set; } = new();
    
    public void AddRecycleBin(RecycleBin recycleBin)
    {
        RecycleBins.Add(recycleBin);
    }
    
    public RecycleBin GetRecycleBinByUser(User user)
    {
        foreach (RecycleBin recycleBin in RecycleBins)
        {
            if (recycleBin.User.Id == user.Id)
            {
                return recycleBin;
            }
        }
        RecycleBin newRecycleBin = new RecycleBin(user, 10);
        RecycleBins.Add(newRecycleBin);
        return newRecycleBin;
    }
    
    public void RemoveRecycleBinByUser(User user)
    {
        RecycleBin recycleBin = GetRecycleBinByUser(user);
        RecycleBins.Remove(recycleBin);
    }
}