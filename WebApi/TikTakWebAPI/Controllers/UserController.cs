using Microsoft.AspNetCore.Mvc;
using TikTakWebAPI.Models;

namespace TikTakWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController{


    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger){
        _logger = logger;
    }

    // Create User
    [HttpPut()]
    public IActionResult AddUser(){
        
        return new OkResult();
    }

    // Delete User
    [HttpDelete()]
    public IActionResult DeleteUser(){

        return new OkResult();
    }

    // Get User
    [HttpGet("GetUserByUsername")]
    public User GetUserByUsername(string username){
        return null;
    }

    // Get User's videos
    [HttpGet("GetAllUserVideos")]
    public List<Video> GetAllVideoFromUser(string username){
        return null;
    }

    // 
}