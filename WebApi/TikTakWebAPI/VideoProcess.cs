using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TikTakWebAPI;

public class VideoProcess
{

    private BlockingCollection<(string, string)> videosPaths = new BlockingCollection<(string, string)>();

    private readonly ILogger<VideoProcess> _logger;

    public VideoProcess(ILogger<VideoProcess> logger)
    {
        _logger = logger;

        Task.Run(() =>
        {
            foreach (var videotuple in videosPaths.GetConsumingEnumerable())
            {
                string filename = Path.GetFileNameWithoutExtension(videotuple.Item1);
                
                _logger.LogInformation($"Starting proccessing on {filename}");

                Video240p(videotuple.Item1, videotuple.Item2);
                _logger.LogInformation($"240p done for  {filename}");
                Video360p(videotuple.Item1, videotuple.Item2);
                _logger.LogInformation($"360p done for  {filename}");
                Video480p(videotuple.Item1, videotuple.Item2);
                _logger.LogInformation($"480p done for  {filename}");
                Video720p(videotuple.Item1, videotuple.Item2);
                _logger.LogInformation($"720p done for  {filename}");
                Video1080p(videotuple.Item1, videotuple.Item2);
                _logger.LogInformation($"1080p done for  {filename}");
                MakeIndexFile(videotuple.Item1, videotuple.Item2, filename);


                File.Delete(videotuple.Item1);
                
                _logger.LogInformation("Commands executed");

            }
        });
    }

    public void AddVideo(string filepath, string outputDir)
    {
        videosPaths.Add((filepath, outputDir));
    }

    private void Video240p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i \"{filepath}\" " +
        "-vf scale=w=240:h=426:force_original_aspect_ratio=decrease,pad=240:426:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264_nvenc -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 240k " +
        "-maxrate 240k -bufsize 480k -b:a 64k -hls_segment_filename " + outputDir + "/240p_%d.ts " + outputDir + "/240p.m3u8";


        RunCommand(command);
    }

    private void Video360p(string filepath, string outputDir)
    {
        string command = $"ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i \"{filepath}\" " +
        "-vf scale=w=360:h=640:force_original_aspect_ratio=decrease,pad=360:640:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264_nvenc -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 800k " +
        "-maxrate 856k -bufsize 1200k -b:a 96k -hls_segment_filename " + outputDir + "/360p_%d.ts " + outputDir + "/360p.m3u8";

        RunCommand(command);
    }

    private void Video480p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i \"{filepath}\" " +
        "-vf scale=w=480:h=842:force_original_aspect_ratio=decrease,pad=480:842:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264_nvenc -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 1400k " +
        "-maxrate 1498k -bufsize 2100k -b:a 128k -hls_segment_filename " + outputDir + "/480p_%d.ts " + outputDir + "/480p.m3u8";

        RunCommand(command);
    }

    private void Video720p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i \"{filepath}\" " +
        "-vf scale=w=720:h=1280:force_original_aspect_ratio=decrease,pad=720:1280:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264_nvenc -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 2800k " +
        "-maxrate 2996k -bufsize 4200k -b:a 128k -hls_segment_filename " + outputDir + "/720p_%d.ts " + outputDir + "/720p.m3u8";

        RunCommand(command);
    }

    private void Video1080p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i \"{filepath}\" " +
        "-vf scale=w=1080:h=1920:force_original_aspect_ratio=decrease,pad=1080:1920:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264_nvenc -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 5000k " +
        "-maxrate 5350k -bufsize 7500k -b:a 192k -hls_segment_filename " + outputDir + "/1080p_%d.ts " + outputDir + "/1080p.m3u8";

        RunCommand(command);
    }

    private void MakeIndexFile(string filepath, string outputDir, string videoname)
    {
        _logger.LogInformation(filepath);
        //Create index as master playlist  
        string path = outputDir + "/index.m3u8";
        File.Create(path).Dispose();
        string[] line ={
                    "#EXTM3U",
                    "#EXT-X-VERSION:3",
                    "#EXT-X-STREAM-INF:BANDWIDTH=10000,RESOLUTION=240x426",
                    $"../stream/{videoname}/240p.m3u8",
                    "#EXT-X-STREAM-INF:BANDWIDTH=420000,RESOLUTION=360x640",
                    $"../stream/{videoname}/360p.m3u8",
                    "#EXT-X-STREAM-INF:BANDWIDTH=680000,RESOLUTION=480x842",
                    $"../stream/{videoname}/480p.m3u8",
                    "#EXT-X-STREAM-INF:BANDWIDTH=1256000,RESOLUTION=720x1280",
                    $"../stream/{videoname}/720p.m3u8",
                    "#EXT-X-STREAM-INF:BANDWIDTH=2000000,RESOLUTION=1080x1920",
                    $"../stream/{videoname}/1080p.m3u8"
                };
        File.WriteAllLines(path, line);
    }

    private void RunCommand(string command)
    {
        ProcessStartInfo ProcessInfo;
        Process Process;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            ProcessInfo = new ProcessStartInfo("foot", command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.RedirectStandardError = true;

        }
        else
        {
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/c" + command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
        }

        Process = Process.Start(ProcessInfo);
        //string errors = Process.StandardError.ReadToEnd();
        //_logger.LogError(errors);
        Process.WaitForExit();
    }

}