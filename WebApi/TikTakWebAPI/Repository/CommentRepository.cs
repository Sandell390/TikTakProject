namespace TikTakWebAPI.Repository;

using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;

public class CommentRepository(DatabaseManager databaseManager) : ICommentRepository
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public bool AddComment(Comment comment)
    {
        string sql = "INSERT INTO comments (video_id, googleid, comment) VALUES (@video_id, @googleid, @comment)";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "video_id", comment.VideoID },
            {"googleid", comment.UserID},
            {"comment", comment.CommentText}
        };

        if (comment.ParentIDComment > 0){
            parameters.Add("parent_comment_id",comment.ParentIDComment);
            sql = "INSERT INTO comments (video_id, googleid, parent_comment_id, comment) VALUES (@video_id, @googleid, @parent_comment_id, @comment)";
        }

        return _databaseManager.NonQuery(sql, parameters);
    }

    public bool DeleteComment(long commentId)
    {
        string sql = "DELETE FROM comments where id = @id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "id", commentId}
        };

        return _databaseManager.NonQuery(sql, parameters);
    }

    public Comment GetCommentById(long commentId)
    {
        string sql = "SELECT * FROM comments WHERE id = @id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "id", commentId }
        };

        Comment comment = _databaseManager.Query(sql, reader => new Comment(
            (long)reader["id"], 
            reader["video_id"].ToString(), 
            reader["googleid"].ToString(), 
            reader["parent_comment_id"] is System.DBNull ? -1 : (long)reader["parent_comment_id"], 
            reader["comment"].ToString(),
            DateTime.Parse(reader["creationtime"].ToString())), 
            out bool success, 
            parameters)?.FirstOrDefault()!;

        return comment;
    }

    public List<Comment> GetCommentsByParentCommentId(long parentCommentId)
    {
        string sql = "SELECT id FROM comments WHERE parent_comment_id = @parent_comment_id";
        Dictionary<string, object> parameters = new Dictionary<string, object>{
            {"parent_comment_id", parentCommentId}
        };

        List<long> commentIds = _databaseManager.Query(sql, reader => (long)reader[0], out bool success, parameters).ToList();

        List<Comment> comments = new List<Comment>();
        foreach (long id in commentIds){
            comments.Add(GetCommentById(id));
        }

        return comments;
    }

    public List<Comment> GetCommentsByGoogleId(string googleid)
    {
        string sql = "SELECT id FROM comments WHERE googleid = @googleid";
        Dictionary<string, object> parameters = new Dictionary<string, object>{
            {"googleid", googleid}
        };

        List<long> commentIds = _databaseManager.Query(sql, reader => (long)reader[0], out bool success, parameters).ToList();

        List<Comment> comments = new List<Comment>();
        foreach (long id in commentIds){
            comments.Add(GetCommentById(id));
        }

        return comments;
    }

    public List<Comment> GetCommentsByVideoId(string videoId)
    {
        string sql = "SELECT id FROM comments WHERE video_id = @video_id";
        Dictionary<string, object> parameters = new Dictionary<string, object>{
            {"video_id", videoId}
        };

        List<long> commentIds = _databaseManager.Query(sql, reader => (long)reader["id"], out bool success, parameters).ToList();

        List<Comment> comments = new List<Comment>();
        foreach (long id in commentIds){
            comments.Add(GetCommentById(id));
        }

        return comments;
    }

    public bool UpdateComment(Comment comment)
    {
        throw new NotImplementedException();
    }
}