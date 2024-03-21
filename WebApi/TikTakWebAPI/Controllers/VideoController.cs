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

    
    // Delete Video
    [HttpDelete("DeleteVideo")]
    public IActionResult DeleteUser(string videoId){

        if (!videoRepository.DeleteVideo(videoId))
            return new BadRequestResult();

        return new OkResult();
    }

    // Get Video
    [HttpGet("GetVideoById")]
    public Video GetUserByUsername(string videoId){
        return videoRepository.GetVideoByVideoId(videoId);
    }
    
    /*
    // Update Video
    [HttpPut("UpdateVideo")]
    public IActionResult UpdateUser(string videoId){
        
        Video video = videoRepository.GetVideoByVideoId(videoId);
        if (video == null)
            return new BadRequestResult();

        
        if(!videoRepository.UpdateVideo(video)){
            return new BadRequestResult();
        }
        
        return new OkResult();
    }
    */
}