using Microsoft.AspNetCore.Mvc;

namespace TikTakWebAPI.Controllers;

public class UserController{


    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger){
        _logger = logger;
    }

    // Create User
    [HttpPut()]
    public void AddUser(){
        
    }

    // Delete User

    // Get User

    // Get User's videos

    // 
}