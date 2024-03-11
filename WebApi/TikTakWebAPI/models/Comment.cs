namespace TikTakWebAPI.Models;

public class Comment(string videoID, string userID, string parentIDComment, string comment,DateTime creationTime ){
    public string VideoID { get; private set; } = videoID;
    public string UserID { get; private set; } = userID;
    public string ParentIDComment { get; private set; } = parentIDComment;
    public DateTime CreationTime { get; private set; } = creationTime;
}