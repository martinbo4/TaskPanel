using Domain;

namespace Memory;

public class UserMemory
{
    public List<User> UserList { get; } = new();

    public void AddUser(User user)
    {
        UserList.Add(user);
    }
    
    public void DeleteUser(User user)
    {
        if (!UserList.Contains(user))
            throw new ArgumentException("The user you are trying to delete is not in the system.");
        
        UserList.Remove(user);
    }
    
    public User GetUserByEmail(string? email)
    {
        User user = UserList.FirstOrDefault(u => u.LogIn!.Email == email)!;
        if (user == null)
            throw new ArgumentException("Incorrect email.");

        return user;
    }
    
    public bool UserExistence(string? email)
    {
        foreach (User user in UserList)
        {
            if (user.LogIn!.Email == email)
                return true;
        }
        return false;
    }
    
    public User GetUserById (int id)
    {
        User user = UserList.FirstOrDefault(u => u.Id == id) ?? 
                    throw new ArgumentException("Does not exist a user with this id.");
        
        return user;
    }
}