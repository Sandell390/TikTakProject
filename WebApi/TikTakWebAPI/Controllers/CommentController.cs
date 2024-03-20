namespace TikTakWebAPI.Controllers;

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

}