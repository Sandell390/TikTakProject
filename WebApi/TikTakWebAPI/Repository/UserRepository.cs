namespace TikTakWebAPI.Repository;

using System.Text.Json;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;


public class UserRepository(DatabaseManager databaseManager) : IUserRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public bool AddUser(User user)
    {
        string sql = "INSERT INTO users (username, googleId, avatarImgUrl, bio) values (@username, @googleId, @avatarImgUrl, @bio)";
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "username", user.Username! },
            { "googleId", user.GoogleId! },
            { "avatarImgUrl", user.AvatarImageUrl! },
            { "bio", user.Bio! }
        };

        _databaseManager.Query(sql, reader => true, out bool success, parameters);
        return success;
    }

    public bool DeleteUser(string username)
    {
        string sql = "DELETE FROM users WHERE username = @username";
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "username", username}
        };
        _databaseManager.Query(sql, reader => true, out bool success, parameters);
        return success;
    }

    public User GetUserByGoogleId(string googleId)
    {
        string sql = "SELECT * FROM User WHERE googleId = @googleId";
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "googleId", googleId }
        };
        User user = _databaseManager.Query(sql, reader => new User(
            reader["username"].ToString()!, reader["googleId"].ToString()!, 
            reader["avatarImgUrl"].ToString()!, reader["bio"].ToString()!, 
            reader["deletionRequestTime"] != null), 
            out bool success, parameters)?.FirstOrDefault()!;
        return user;
    }

    public User GetUserByUsername(string username)
    {
        string sql = "SELECT * FROM users WHERE username = @username";
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "username", username }
        };
        User user = _databaseManager.Query(sql, reader => new User(
            reader["username"].ToString()!, 
            reader["googleId"].ToString()!, 
            reader["avatarImgUrl"].ToString()!, 
            reader["bio"].ToString()!, 
            reader["deletionRequestTime"] != null), 
            out bool success, parameters)?.FirstOrDefault()!;
        return user;
    }

    public bool UpdateUser(User user)
    {
        string sql = "UPDATE users SET avatarImgUrl = @avatarImgUrl, bio = @bio WHERE username = @username";
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "username", user.Username! },
            { "avatarImgUrl", user.AvatarImageUrl! },
            { "bio", user.Bio! }
        };

        _databaseManager.Query(sql, reader => true, out bool success, parameters);
        return success;
    }

    public bool SetDeletionRequestByUsername(bool deletionRequest, string username)
    {
        string sql = "UPDATE users SET deletionRequestTime = @deletionRequestTime WHERE username = @username";
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        if (deletionRequest)
            parameters.Add("deletionRequestTime", DateTime.Now.ToString());
        else
            parameters.Add("deletionRequestTime", null);

        parameters.Add("username", username);

        _databaseManager.Query(sql, reader => true, out bool success, parameters);
        return success;
    }
}