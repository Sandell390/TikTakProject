namespace TikTakWebAPI.Repository;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;


public class VideoRepository(DatabaseManager databaseManager) : IVideoRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;
    
    public bool AddVideo(Video video)
    {
        string sql = "INSERT INTO videos (id, description, title, googleId, isVideoPublic) values (@id, @description, @title, @googleId, @isVideoPublic)";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"id",video.ID},
            {"description", video.Description},
            {"title",video.Title},
            {"googleId",video.OwnerGoogleID},
            {"isVideoPublic", video.isPublic}
        };

        _databaseManager.Query(sql, reader => true, out bool success, parameters);
        return success;
    }

    public bool DeleteVideo(Video video)
    {
        throw new NotImplementedException();
    }

    public Video GetVideoByVideoId(string videoId)
    {
        throw new NotImplementedException();
    }

    public List<Video> GetVideosByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public bool UpdateVideo(Video video)
    {
        throw new NotImplementedException();
    }
}