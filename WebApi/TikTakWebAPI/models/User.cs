using System.Numerics;

namespace TikTakWebAPI.Models;

public class User(string username, string googleid, string avatarImageUrl, string bio, bool isToBeDeleted){

    public string? Username { get; set; } = username;
    public string? GoogleId { get; set; } = googleid;
    public string? AvatarImageUrl { get; set; } = avatarImageUrl;
    public string? Bio { get; set; } = bio;
    public bool IsToBeDeleted { get; set; } = isToBeDeleted;

}