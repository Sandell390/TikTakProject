using System.Numerics;

namespace TikTakWebAPI.Models;

public class User(string username, string googleid, string avatarImageUrl, string bio, bool isToBeDeleted){

    public string? Username { get; private set; } = username;
    public string? GoogleId { get; private set; } = googleid;
    public string? AvatarImageUrl { get; private set; } = avatarImageUrl;
    public string? Bio { get; private set; } = bio;
    public bool IsToBeDeleted { get; private set; } = isToBeDeleted;

}