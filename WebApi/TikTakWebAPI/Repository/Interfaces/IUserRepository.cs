using TikTakWebAPI.Models;

namespace TikTakWebAPI.Repository;

public interface IUserRepository{
    bool AddUser(User user);
    bool DeleteUser(string username);
    bool UpdateUser(User user);
    User GetUserByUsername(string username);
    User GetUserByGoogleId(string googleId);

    public bool SetDeletionRequestByUsername(bool deletionRequest, string username);

}