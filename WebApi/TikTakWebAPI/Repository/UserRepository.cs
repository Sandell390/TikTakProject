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
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "username", user.Username! },
            { "googleId", user.GoogleId! },
            { "avatarImgUrl", user.AvatarImageUrl! },
            { "bio", user.Bio! }
        };

        return _databaseManager.NonQuery(sql, parameters);
    }

    public bool DeleteUser(string googleId)
    {
        string sql = "DELETE FROM users WHERE googleId = @googleId";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "googleId", googleId}
        };
        return _databaseManager.NonQuery(sql, parameters);
    }

    public User GetUserByGoogleId(string googleId)
    {
        string sql = "SELECT * FROM users WHERE googleId = @googleId";
        Dictionary<string, object> parameters = new Dictionary<string, object>
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
        Dictionary<string, object> parameters = new Dictionary<string, object>
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
        string sql = "UPDATE users SET avatarImgUrl = @avatarImgUrl, bio = @bio WHERE googleId = @googleId";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "googleId", user.GoogleId! },
            { "avatarImgUrl", user.AvatarImageUrl! },
            { "bio", user.Bio! }
        };

        return _databaseManager.NonQuery(sql, parameters);
    }

    public bool SetDeletionRequestByUsername(bool deletionRequest, string googleId)
    {
        string sql = "UPDATE users SET deletionRequestTime = @deletionRequestTime WHERE googleId = @googleId";
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        if (deletionRequest)
            parameters.Add("deletionRequestTime", DateTime.Now.ToString());
        else
            parameters.Add("deletionRequestTime", null);

        parameters.Add("googleId", googleId);

        return _databaseManager.NonQuery(sql, parameters);
    }
}