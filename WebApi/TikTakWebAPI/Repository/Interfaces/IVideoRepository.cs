namespace TikTakWebAPI.Repository;
using TikTakWebAPI.Models;

public interface IVideoRepository{
    
    bool AddVideo(Video video);
    bool DeleteVideo(Video video);
    bool UpdateVideo(Video video);
    Video GetVideoByVideoId(string videoId);
    List<Video> GetVideosByUsername(string username);
}