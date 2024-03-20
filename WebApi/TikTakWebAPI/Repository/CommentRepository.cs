namespace TikTakWebAPI.Repository;

using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;

public class CommentRepository(DatabaseManager databaseManager) : ICommentRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public bool AddComment(Comment comment)
    {
        throw new NotImplementedException();
    }

    public bool DeleteComment(string commentId)
    {
        throw new NotImplementedException();
    }

    public Comment GetCommentById(string id)
    {
        throw new NotImplementedException();
    }

    public List<Comment> GetCommentsByParentCommentId(string parentCommentId)
    {
        throw new NotImplementedException();
    }

    public List<Comment> GetCommentsByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public List<Comment> GetCommentsByVideoId(string videoId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateComment(Comment comment)
    {
        throw new NotImplementedException();
    }
}