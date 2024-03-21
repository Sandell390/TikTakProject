namespace TikTakWebAPI.Models;

public class Video(string id, string title, string description,string ownerGoogleID, int likes, List<long>? commentIDs, bool isPublic, DateTime uploadTime){
    public string ID { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public string OwnerGoogleID { get; set; } = ownerGoogleID;
    public int Likes { get; set; } = likes;
    public List<long>? CommentIDs { get; set; } = commentIDs == null ? new List<long>() : commentIDs;
    public bool isPublic { get; set; } = isPublic;
    public DateTime UploadTime { get; set; } = uploadTime;
}