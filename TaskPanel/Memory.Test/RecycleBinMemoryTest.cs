using Domain;

namespace Memory.Test;

[TestClass]
public class RecycleBinMemoryTest
{
    [TestMethod]
    public void RecycleBinsIsNotNullAfterConstructor()
    {
        RecycleBinMemory recycleBinMemory = new RecycleBinMemory();

        List<RecycleBin> recycleBins = recycleBinMemory.RecycleBins;

        Assert.IsNotNull(recycleBins);
    }

    [TestMethod]
    public void RecycleBinsAddOneTask()
    {
        RecycleBinMemory recycleBinMemory = new RecycleBinMemory();
        User user = new User();

        recycleBinMemory.AddRecycleBin(new RecycleBin(user));

        Assert.AreEqual(1, recycleBinMemory.RecycleBins.Count);
    }

    [TestMethod]
    public void GetOneRecycleBinByUserId()
    {
        RecycleBinMemory recycleBinMemory = new RecycleBinMemory();
        User user = new User();
        recycleBinMemory.AddRecycleBin(new RecycleBin(user));

        RecycleBin recycleBin = recycleBinMemory.GetRecycleBinByUser(user);

        Assert.AreEqual(user.Id, recycleBin.User.Id);
    }
    
    [TestMethod]
    public void RemoveOneRecycleBinByUserId()
    {
        RecycleBinMemory recycleBinMemory = new RecycleBinMemory();
        User user = new User();
        recycleBinMemory.AddRecycleBin(new RecycleBin(user));

        recycleBinMemory.RemoveRecycleBinByUser(user);

        Assert.AreEqual(0, recycleBinMemory.RecycleBins.Count);
    }
    
    [TestMethod]
    public void SetRecycleBins_ShouldUpdateRecycleBins()
    {
        RecycleBinMemory recycleBinMemory = new RecycleBinMemory();
        User user = new User();
        RecycleBin recycleBin = new RecycleBin(user);
        List<RecycleBin> newRecycleBins = new List<RecycleBin> { recycleBin };

        recycleBinMemory.RecycleBins = newRecycleBins;

        Assert.AreEqual(newRecycleBins, recycleBinMemory.RecycleBins);
    }
}