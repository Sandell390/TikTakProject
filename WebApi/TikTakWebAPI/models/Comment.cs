namespace TikTakWebAPI.Models;

public class Comment(long commentID,string videoID, string userID, long parentIDComment, string commentText,DateTime creationTime ){


    public long CommentID { get; private set; } = commentID;
    public string VideoID { get; private set; } = videoID;
    public string UserID { get; private set; } = userID;
    public long ParentIDComment { get; private set; } = parentIDComment == null ? 0 : parentIDComment;
    public string CommentText {get; private set;} = commentText;
    public DateTime CreationTime { get; private set; } = creationTime;
}