namespace TikTakWebAPI.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using TikTakWebAPI.Repository;

[ApiController]
[Route("[controller]")]
public class CommentController{

    private readonly ILogger<CommentController> _logger;

    private readonly ICommentRepository commentRepository;

    public CommentController(ILogger<CommentController> logger){
        _logger = logger;
        commentRepository = new CommentRepository(new DAL.DatabaseManager(_logger));
    }

    [HttpPut("AddComment")]
    public IActionResult AddComment(string googleId, string videoId, string commentText, long parent_comment_id = -1){
        Comment comment = new Comment(0, videoId, googleId, parent_comment_id, commentText, DateTime.Now);

        if(!commentRepository.AddComment(comment)){
            return new BadRequestResult();
        }

        return new OkResult();
    }


    [HttpDelete("DeleteComment")]
    public IActionResult DeleteComment(long commentId){
        if (!commentRepository.DeleteComment(commentId)){
            return new BadRequestResult();
        }

        return new OkResult();
    }

    [HttpGet("GetCommentById")]
    public Comment GetCommentById(long commentId){
        return commentRepository.GetCommentById(commentId);
    }

    [HttpGet("GetCommentsByGoogleId")]
    public List<Comment> GetCommentsByGoogleId(string googleId){
        return commentRepository.GetCommentsByGoogleId(googleId);
    }

    [HttpGet("GetCommentsByVideoId")]
    public List<Comment> GetCommentsByVideoId(string videoId){
        return commentRepository.GetCommentsByVideoId(videoId);
    }

    [HttpGet("GetCommentsByParentCommentId")]
    public List<Comment> GetCommentsByParentCommentId(long parentCommentId){
        return commentRepository.GetCommentsByParentCommentId(parentCommentId);
    }

    [HttpPost("UpdateComment")]
    public IActionResult UpdateComment(long commentId){
        return new ForbidResult();
    }


}