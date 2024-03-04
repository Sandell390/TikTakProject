using Microsoft.AspNetCore.Http.HttpResults;

namespace TikTakWebAPI.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("[controller]")]
public class TranscodeVideoController
{

    private readonly ILogger<TranscodeVideoController> _logger;
    private readonly IWebHostEnvironment _env;

    
    public TranscodeVideoController(ILogger<TranscodeVideoController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }


   [HttpPut(Name = "AddVideo")]
public async Task<IActionResult> Put(IFormFile file)
{
    _logger.LogInformation("Received a PUT request");

    if (file == null || file.Length == 0)
    {
        _logger.LogWarning("No file received");
        return new BadRequestResult();
    }

    var filePath = Path.Combine("Videos", file.FileName);

    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    _logger.LogInformation("Saved the video at {FilePath}", filePath);

    var outputDir = Path.Combine("Videos", Path.GetFileNameWithoutExtension(file.FileName));
    Directory.CreateDirectory(outputDir);

    var command = $"ffmpeg -i {filePath} -profile:v baseline -level 3.0 -s 640x360 -start_number 0 -hls_time 10 -hls_list_size 0 -f hls {outputDir}/index.m3u8";

    _logger.LogInformation("Running command: {Command}", command);

    ProcessStartInfo ProcessInfo;
    Process Process;

    ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
    ProcessInfo.CreateNoWindow = true;
    ProcessInfo.UseShellExecute = false;

    Process = Process.Start(ProcessInfo);
    Process.WaitForExit();

    _logger.LogInformation("Command executed");

    return new OkResult();
}

[HttpGet("stream/{name}")]
public IActionResult GetHlsPlaylist(string name)
{
    if (name.EndsWith(".ts"))
    {
        return GetHlsSegment(name);
    }

    _logger.LogInformation("Received a GET request for HLS playlist: {Name}", name);

    var filePath = Path.Combine(_env.ContentRootPath, "Videos", name, "index.m3u8");
    if (!System.IO.File.Exists(filePath))
    {
        _logger.LogWarning("File not found: {FilePath}", filePath);
        return new NotFoundResult();
    }

    _logger.LogInformation("Returning file: {FilePath}", filePath);
    return new PhysicalFileResult(filePath, "application/vnd.apple.mpegurl");
}

[HttpGet("GetVideoList")]
public List<string?> GetVideoList()
{
    var videoFolderPath = Path.Combine(_env.ContentRootPath, "Videos");
    var directories = Directory.GetDirectories(videoFolderPath);
    var videoNames = directories.Select(Path.GetFileName).ToList();
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
    
}