namespace TikTakWebAPI.Models;

public class Video(string id, string title, string description,User owner, int likes, List<Comment> comments, bool isPublic, DateTime uploadTime){
    public string ID { get; private set; } = id;
    public string Title { get; private set; } = title;
    public string description { get; private set; } = description;
    public User Owner { get; private set; } = owner;
    public int Likes { get; private set; } = likes;
    public List<Comment> Comments { get; private set; } = comments;
    public bool isPublic { get; private set; } = isPublic;
    public DateTime UploadTime { get; private set; } = uploadTime;
}