using Microsoft.AspNetCore.Http.HttpResults;

namespace TikTakWebAPI.Controllers;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("[controller]")]
public class TranscodeVideoController
{

    private readonly ILogger<TranscodeVideoController> _logger;
    private readonly IWebHostEnvironment _env;

    private readonly VideoProcess _videoProcess;

    public TranscodeVideoController(ILogger<TranscodeVideoController> logger, IWebHostEnvironment env, VideoProcess videoProcess)
    {
        _videoProcess = videoProcess;
        _logger = logger;
        _env = env;
    }


    [HttpPut("AddVideo")]
    public async Task<String> Put(IFormFile file)
    {
        _logger.LogInformation("Received a PUT request");

        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("No file received");
            return "No file received";
        }

        Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Videos"));

        string fileId = Guid.NewGuid().ToString();

        var filePath = Path.Combine("Videos", fileId + Path.GetExtension(file.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _logger.LogInformation("Saved the video at {FilePath}", filePath);

        var outputDir = Path.Combine("Videos", fileId);
        _ = Directory.CreateDirectory(outputDir);

        _videoProcess.AddVideo(filePath,outputDir);

        return "File received, the server is proccessing the video";
    }

    [HttpGet("stream/{name}")]
    public IActionResult GetHlsPlaylist(string name)
    {
        if (name.EndsWith(".ts"))
        {
            return GetHlsSegment(name);
        }

        string filePath = "";
        _logger.LogInformation("Received a GET request for HLS playlist: {Name}", name);

        filePath = Path.Combine(_env.ContentRootPath, "Videos", name, "index.m3u8");
        if (!System.IO.File.Exists(filePath))
        {
            _logger.LogWarning("File not found: {FilePath}", filePath);
            return new NotFoundResult();
        }

        if (File.Exists(Path.Combine(_env.ContentRootPath, "Videos", name + ".mp4"))){
            _logger.LogWarning("Client trying to request video while it is processing");
            return new NotFoundResult();
        }

        _logger.LogInformation($"Returning file: {filePath}");

        return new PhysicalFileResult(filePath, "application/vnd.apple.mpegurl");
    }

    [HttpGet("GetVideoList")]
    public List<string?> GetVideoList()
    {
        var videoFolderPath = Path.Combine(_env.ContentRootPath, "Videos");
        var directories = Directory.GetDirectories(videoFolderPath);
        var videoNames = directories.Select(Path.GetFileName).ToList();
        videoNames = videoNames.Where(x => !File.Exists(Path.Combine(_env.ContentRootPath, "Videos", x + ".mp4"))).ToList();
        return videoNames;
    }

    [HttpGet("stream/{name}/{segment}")]
    public IActionResult GetHlsSegment(string name, string segment)
    {
        return GetHlsSegment(Path.Combine(name, segment));
    }

    private IActionResult GetHlsSegment(string path)
    {
        _logger.LogInformation("Received a GET request for HLS segment: {Path}", path);

        var filePath = Path.Combine(_env.ContentRootPath, "Videos", path);
        if (!System.IO.File.Exists(filePath))
        {
            _logger.LogWarning("File not found: {FilePath}", filePath);
            return new NotFoundResult();
        }

        _logger.LogInformation("Returning file: {FilePath}", filePath);
        return new PhysicalFileResult(filePath, "video/MP2T");
    }

    [HttpGet("GetVideoProcessStatus/{name}")]
    public IActionResult GetVideoProcessStatus(string name){
        
        var filePath = Path.Combine(_env.ContentRootPath, "Videos", name);

        if (!Directory.Exists(filePath)){
            return new BadRequestObjectResult("Video does not exists");
        }

        List<string> files = Directory.GetFiles(filePath, "*.m3u8").ToList();

        for (int i = 0; i < files.Count; i++)
        {
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }
        int percent;

        foreach (var item in files){
            _logger.LogDebug(item);
        }
        if (files.Contains("index")){
            percent = 100;
        }else{
            percent = files.Count * 20;
        }

        return new OkObjectResult(percent);
    }
}

// Video needs to have a unique id 

