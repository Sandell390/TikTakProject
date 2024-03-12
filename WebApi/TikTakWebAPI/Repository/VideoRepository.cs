namespace TikTakWebAPI.Repository;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;


public class VideoRepository(DatabaseManager databaseManager) : IVideoRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;
    
    public bool AddVideo(Video video)
    {
        throw new NotImplementedException();
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