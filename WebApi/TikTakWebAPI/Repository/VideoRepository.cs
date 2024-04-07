namespace TikTakWebAPI.Repository;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;


public class VideoRepository(DatabaseManager databaseManager) : IVideoRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;
    
    public bool AddVideo(Video video)
    {
        // Test to see how gitbutler works
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

    public bool DeleteVideo(string videoId)
    {
        string sql = "DELETE FROM videos WHERE id = @videoId";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "videoId", videoId}
        };
        return _databaseManager.NonQuery(sql, parameters);
    }

    public Video GetVideoByVideoId(string videoId)
    {
        string sql = "SELECT * FROM videos WHERE id = @videoid";
        Dictionary<string, object> parameters = new Dictionary<string, object>{
            {"videoid", videoId}
        };

        Video video = _databaseManager.Query(sql, reader => new Video(reader["id"].ToString(), reader["title"].ToString(), reader["description"].ToString(), reader["googleid"].ToString(), 0, null, (bool)reader["isvideopublic"], DateTime.Parse(reader["uploadtime"].ToString())), out bool success, parameters).FirstOrDefault();

        
        sql = "SELECT COUNT(*) FROM likes WHERE videoid = @videoid";

        long likes = (long)_databaseManager.QueryScalar(sql, parameters);

        video.Likes = (int)likes;

        sql = "SELECT * FROM comments WHERE video_id = @videoid";

        List<Comment> comments = _databaseManager.Query(sql, reader => new Comment((long)reader["id"],reader["video_id"].ToString(), reader["googleId"].ToString(), (long)reader["parent_comment_id"], reader["comment"].ToString(), DateTime.Parse(reader["creationTime"].ToString())), out success, parameters).ToList();

        foreach (Comment comment in comments)
        {
            video.CommentIDs.Add(comment.CommentID);
        }

        return video;
    }

    public List<Video> GetVideosByUsername(string googleId)
    {
        string sql = "SELECT id FROM videos WHERE googleId = @googleId";
        Dictionary<string, object> parameters = new Dictionary<string, object>{
            {"googleId", googleId}
        };


        List<string> videoIds = _databaseManager.Query(sql, reader => (string)reader[0], out bool success, parameters).ToList();

        List<Video> userVideos = new List<Video>();
        foreach (string videoid in videoIds){
            userVideos.Add(GetVideoByVideoId(videoid));
        }
        
        return userVideos;
    }

    public bool UpdateVideo(Video video)
    {
        throw new NotImplementedException();
    }
}