namespace TikTakWebAPI.Models;

public class Video(string id, string title, string description,string ownerGoogleID, int likes, List<string>? commentIDs, bool isPublic, DateTime uploadTime){
    public string ID { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public string OwnerGoogleID { get; set; } = ownerGoogleID;
    public int Likes { get; set; } = likes;
    public List<string>? CommentIDs { get; set; } = commentIDs;
    public bool isPublic { get; set; } = isPublic;
    public DateTime UploadTime { get; set; } = uploadTime;
}