using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;
using TikTakWebAPI.Repository;

namespace TikTakWebAPI.Controllers;

[ApiController]
[Route("")]
public class UserController{


    private IUserRepository userRepository;
    private IVideoRepository videoRepository;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger){
        _logger = logger;
        userRepository = new UserRepository(new DAL.DatabaseManager(_logger));
        videoRepository = new VideoRepository(new DAL.DatabaseManager(_logger));
    }

    // Create User
    [HttpPut("AddUser")]
    public IActionResult AddUser(User newUser){

        if (!userRepository.AddUser(newUser))
            return new BadRequestResult();

        return new OkResult();
    }

    // Delete User
    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(string username){

        if (!userRepository.DeleteUser(username))
            return new BadRequestResult();

        return new OkResult();
    }

    // Get User
    [HttpGet("GetUserByUsername")]
    public User GetUserByUsername(string username){
        return userRepository.GetUserByUsername(username);
    }

    [HttpGet("GetUserByGoogleId")]
    public User GetUserByGoogleId(string googleId){
        return userRepository.GetUserByGoogleId(googleId);
    }

    // Get User's videos
    [HttpGet("{username}/Videos")]
    public List<Video> GetAllVideoFromUser(string username){
        return videoRepository.GetVideosByUsername(username);
    }

    // Update User
    [HttpPut("{username}/UpdateBio")]
    public IActionResult UpdateUser(string username, string bio){
        
        User user = userRepository.GetUserByUsername(username);
        if (user == null)
            return new BadRequestResult();

        user.Bio = bio;
        
        if(!userRepository.UpdateUser(user)){
            return new BadRequestResult();
        }
        
        return new OkResult();
    }
}