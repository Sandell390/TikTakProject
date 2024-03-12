namespace TikTakWebAPI.Repository;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;


public class UserRepository(DatabaseManager databaseManager) : IUserRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public bool AddUser(User user)
    {
        throw new NotImplementedException();
    }

    public bool DeleteUser(User user)
    {
        throw new NotImplementedException();
    }

    public User GetUserByGoogleId(string googleId)
    {
        throw new NotImplementedException();
    }

    public User GetUserByUsername(string username)
    {
        string sql = "SELECT * FROM User WHERE username = @username";
        //User user = _databaseManager.Query(sql, reader => new User());
        return null;
    }

    public bool UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}