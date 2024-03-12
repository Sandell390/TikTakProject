using TikTakWebAPI.Models;

namespace TikTakWebAPI.Repository;

public interface IUserRepository{
    bool AddUser(User user);
    bool DeleteUser(User user);
    bool UpdateUser(User user);
    User GetUserByUsername(string username);
    User GetUserByGoogleId(string googleId);

}