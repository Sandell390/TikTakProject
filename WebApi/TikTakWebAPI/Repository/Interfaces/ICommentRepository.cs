namespace TikTakWebAPI.Repository;

using TikTakWebAPI.Models;

public interface ICommentRepository{
    bool AddComment(Comment comment);
    bool DeleteComment(long commentId);
    bool UpdateComment(Comment comment);
    List<Comment> GetCommentsByVideoId(string videoId);
    List<Comment> GetCommentsByGoogleId(string googleId);
    List<Comment> GetCommentsByParentCommentId(long parentCommentId);
    Comment GetCommentById(long id);
}