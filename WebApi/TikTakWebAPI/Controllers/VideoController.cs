using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TikTakWebAPI.DAL;
using TikTakWebAPI.Models;
using TikTakWebAPI.Repository;

namespace TikTakWebAPI.Controllers;

[ApiController]
[Route("")]
public class VideoController{


    private IVideoRepository videoRepository;

    private readonly ILogger<VideoController> _logger;

    public VideoController(ILogger<VideoController> logger){
        _logger = logger;
        videoRepository = new VideoRepository(new DAL.DatabaseManager(_logger));
    }

    // Create User
    [HttpPut("AddVideo")]
    public async Task<IActionResult> AddVideoAsync(string title, string desp, string googleid, bool isPublic,IFormFile file){

        string videoid = await ProcessVideo(file);

        Video video = new Video(videoid, title, desp, googleid, 0, null, isPublic, DateTime.Now);

        if (!videoRepository.AddVideo(video))
            return new BadRequestObjectResult("");

        return new OkObjectResult("{\"videoID\": \""+ videoid + "\"}");
    }

    async Task<string> ProcessVideo(IFormFile file){
        HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5050")
        };
        HttpContent fileStreamContent = new StreamContent(file.OpenReadStream());
        var formData = new MultipartFormDataContent
        {
            { fileStreamContent, "file", file.FileName }
        };
        HttpResponseMessage response = await httpClient.PostAsync("TranscodeVideo/AddVideo", formData);

        JsonNode json = JsonNode.Parse(response.Content.ReadAsStream());

        return json["videoID"].ToString();
    }

    /*
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

    // Get User's videos
    [HttpGet("{username}/Videos")]
    public List<Video> GetAllVideoFromUser(string username){
        return null;
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
    */
}