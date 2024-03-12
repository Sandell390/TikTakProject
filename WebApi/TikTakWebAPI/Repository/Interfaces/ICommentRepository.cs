namespace TikTakWebAPI.Repository;

using TikTakWebAPI.Models;

public interface ICommentRepository{
    bool AddComment(Comment comment);
    bool DeleteComment(Comment comment);
    bool UpdateComment(Comment comment);
    List<Comment> GetCommentsByVideoId(string videoId);
    List<Comment> GetCommentsByUsername(string username);
    List<Comment> GetCommentsByParentCommentId(string parentCommentId);
    Comment GetCommentById(string id);
}