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

    _logger.LogInformation("Running commands");

    Video240p(filePath, outputDir);
    Video360p(filePath, outputDir);
    Video480p(filePath, outputDir);
    Video720p(filePath, outputDir);
    Video1080p(filePath, outputDir);
    MakeIndexFile(filePath, outputDir, Path.GetFileNameWithoutExtension(file.FileName));


    _logger.LogInformation("Commands executed");

    return new OkResult();
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




private void Video240p(string filepath, string outputDir){

    string command = $"ffmpeg -i \"{filepath}\" " + 
    "-vf scale=w=426:h=240:force_original_aspect_ratio=decrease -c:a aac "+ 
    "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 "+ 
    "-keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v 240k "+ 
    "-maxrate 240k -bufsize 480k -b:a 64k -hls_segment_filename " + outputDir + "/240p_%d.ts " + outputDir + "/240p.m3u8";


    RunCommand(command);
}

private void Video360p(string filepath, string outputDir){
    string command = $"ffmpeg -i \"{filepath}\" " + 
    "-vf scale=w=640:h=360:force_original_aspect_ratio=decrease -c:a aac "+ 
    "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 "+ 
    "-keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v 800k "+ 
    "-maxrate 856k -bufsize 1200k -b:a 96k -hls_segment_filename " + outputDir + "/360p_%d.ts " + outputDir + "/360p.m3u8";

    RunCommand(command);
}

private void Video480p(string filepath, string outputDir){

    string command = $"ffmpeg -i \"{filepath}\" " + 
    "-vf scale=w=842:h=480:force_original_aspect_ratio=decrease -c:a aac "+ 
    "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 "+ 
    "-keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v 1400k "+ 
    "-maxrate 1498k -bufsize 2100k -b:a 128k -hls_segment_filename " + outputDir + "/480p_%d.ts " + outputDir + "/480p.m3u8";

    RunCommand(command);
}

private void Video720p(string filepath, string outputDir){

    string command = $"ffmpeg -i \"{filepath}\" " + 
    "-vf scale=w=1280:h=720:force_original_aspect_ratio=decrease -c:a aac "+ 
    "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 "+ 
    "-keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v 2800k "+ 
    "-maxrate 2996k -bufsize 4200k -b:a 128k -hls_segment_filename " + outputDir + "/720p_%d.ts " + outputDir + "/720p.m3u8";

    RunCommand(command);
}

private void Video1080p(string filepath, string outputDir){

    string command = $"ffmpeg -i \"{filepath}\" " + 
    "-vf scale=w=1920:h=1080:force_original_aspect_ratio=decrease -c:a aac "+ 
    "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 "+ 
    "-keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v 5000k "+ 
    "-maxrate 5350k -bufsize 7500k -b:a 192k -hls_segment_filename " + outputDir + "/1080p_%d.ts " + outputDir + "/1080p.m3u8";

    RunCommand(command);
}

private void MakeIndexFile(string filepath, string outputDir, string videoname){
                _logger.LogInformation(filepath);
                //Create index as master playlist  
                string path = outputDir + "/index.m3u8";  
                File.Create(path).Dispose();  
                string[] line ={  
                    "#EXTM3U",  
                    "#EXT-X-VERSION:3",  
                    "#EXT-X-STREAM-INF:BANDWIDTH=10000,RESOLUTION=426x240",  
                    $"../stream/{videoname}/240p.m3u8",  
                    "#EXT-X-STREAM-INF:BANDWIDTH=420000,RESOLUTION=640x360",  
                    $"../stream/{videoname}/360p.m3u8",  
                    "#EXT-X-STREAM-INF:BANDWIDTH=680000,RESOLUTION=842x480",  
                    $"../stream/{videoname}/480p.m3u8",  
                    "#EXT-X-STREAM-INF:BANDWIDTH=1256000,RESOLUTION=1280x720",  
                    $"../stream/{videoname}/720p.m3u8",  
                    "#EXT-X-STREAM-INF:BANDWIDTH=2000000,RESOLUTION=1920x1080",  
                    $"../stream/{videoname}/1080p.m3u8"  
                };  
                File.WriteAllLines(path, line);
}

private void RunCommand(string command){
    ProcessStartInfo ProcessInfo;
    Process Process;
    if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
    ProcessInfo = new ProcessStartInfo("foot", command);
    ProcessInfo.CreateNoWindow = true;
    ProcessInfo.UseShellExecute = false;
    ProcessInfo.RedirectStandardError = true;

    }else {
    ProcessInfo = new ProcessStartInfo("cmd.exe","/c" + command);
    ProcessInfo.CreateNoWindow = true;
    ProcessInfo.UseShellExecute = false;
    }

    Process = Process.Start(ProcessInfo);
    string errors = Process.StandardError.ReadToEnd();
    _logger.LogError(errors);
    Process.WaitForExit();
}
    
}