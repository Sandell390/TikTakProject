using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Routing.Constraints;

namespace TikTakWebAPI;

public class VideoProcess
{

    private BlockingCollection<(string, string)> videosPaths = new BlockingCollection<(string, string)>();

    private readonly ILogger<VideoProcess> _logger;

    public VideoProcess(ILogger<VideoProcess> logger)
    {
        _logger = logger;

        Task.Run(async () =>
        {
            foreach (var videotuple in videosPaths.GetConsumingEnumerable())
            {
                string filename = Path.GetFileNameWithoutExtension(videotuple.Item1);

                _logger.LogInformation($"Starting proccessing on {filename}");
                _logger.LogDebug("FilePath:" + videotuple.Item1);
                _logger.LogDebug("Output Dir:" + videotuple.Item2);

                await StartVideoProcessAsync(videotuple);

                _logger.LogInformation($"Done processing on {filename}");
            }
        });
    }

    private async Task StartVideoProcessAsync((string, string) videotuple)
    {
        string filename = Path.GetFileNameWithoutExtension(videotuple.Item1);

        List<Task> tasks = new List<Task>
        {
            ExecuteVideoProcessingTaskAsync(Video240p, videotuple.Item1, videotuple.Item2, "240p"),
            ExecuteVideoProcessingTaskAsync(Video360p, videotuple.Item1, videotuple.Item2, "360p"),
            ExecuteVideoProcessingTaskAsync(Video480p, videotuple.Item1, videotuple.Item2, "480p"),
            ExecuteVideoProcessingTaskAsync(Video720p, videotuple.Item1, videotuple.Item2, "720p"),
            ExecuteVideoProcessingTaskAsync(Video1080p, videotuple.Item1, videotuple.Item2, "1080p"),
            ExecuteVideoProcessingTaskAsync(MakeThumbnail, videotuple.Item1, videotuple.Item2, "thumbnail"),
            MakeIndexFile(videotuple.Item1, videotuple.Item2, filename)
        };

        await Task.WhenAll(tasks);

        File.Delete(videotuple.Item1);

    }

    private async Task ExecuteVideoProcessingTaskAsync(Func<string, string, Task> videoProcessingTask, string filepath, string outputDir, string resolution)
    {
        _logger.LogInformation($"Starting processing {resolution} for file {Path.GetFileName(filepath)}");

        await videoProcessingTask(filepath, outputDir);

        _logger.LogInformation($"Completed processing {resolution} for file {Path.GetFileName(filepath)}");
    }


    public void AddVideo(string filepath, string outputDir)
    {
        videosPaths.Add((filepath, outputDir));
    }

    private Task MakeThumbnail(string filepath, string outputDir)
    {
        string command = $"ffmpeg -i \"{filepath}\" -frames:v 1 {outputDir}/thumbnail.png";

        return RunCommand(command);
    }

    private Task Video240p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -i \"{filepath}\" " +
        "-vf scale=w=240:h=426:force_original_aspect_ratio=decrease,pad=240:426:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 240k " +
        "-maxrate 240k -bufsize 480k -b:a 64k -hls_segment_filename " + outputDir + "/240p_%d.ts " + outputDir + "/240p.m3u8";

        return RunCommand(command);

    }

    private Task Video360p(string filepath, string outputDir)
    {
        string command = $"ffmpeg -i \"{filepath}\" " +
        "-vf scale=w=360:h=640:force_original_aspect_ratio=decrease,pad=360:640:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 800k " +
        "-maxrate 856k -bufsize 1200k -b:a 96k -hls_segment_filename " + outputDir + "/360p_%d.ts " + outputDir + "/360p.m3u8";

        return RunCommand(command);

    }

    private Task Video480p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -i \"{filepath}\" " +
        "-vf scale=w=480:h=842:force_original_aspect_ratio=decrease,pad=480:842:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 1400k " +
        "-maxrate 1498k -bufsize 2100k -b:a 128k -hls_segment_filename " + outputDir + "/480p_%d.ts " + outputDir + "/480p.m3u8";

        return RunCommand(command);

    }

    private Task Video720p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -i \"{filepath}\" " +
        "-vf scale=w=720:h=1280:force_original_aspect_ratio=decrease,pad=720:1280:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 2800k " +
        "-maxrate 2996k -bufsize 4200k -b:a 128k -hls_segment_filename " + outputDir + "/720p_%d.ts " + outputDir + "/720p.m3u8";

        return RunCommand(command);

    }

    private Task Video1080p(string filepath, string outputDir)
    {

        string command = $"ffmpeg -i \"{filepath}\" " +
        "-vf scale=w=1080:h=1920:force_original_aspect_ratio=decrease,pad=1080:1920:(ow-iw)/2:(oh-ih)/2 -c:a aac " +
        "-ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 " +
        "-keyint_min 48 -hls_time 2 -hls_playlist_type vod -b:v 5000k " +
        "-maxrate 5350k -bufsize 7500k -b:a 192k -hls_segment_filename " + outputDir + "/1080p_%d.ts " + outputDir + "/1080p.m3u8";

        return RunCommand(command);

    }

    private Task MakeIndexFile(string filepath, string outputDir, string videoname)
    {
        _logger.LogInformation(filepath);
        string path = outputDir + "/index.m3u8";

        //Create index as master playlist  
        File.Create(path).Dispose();

        string[] line = [
                    "#EXTM3U",
                "#EXT-X-VERSION:3",
                "#EXT-X-STREAM-INF:BANDWIDTH=10000,RESOLUTION=240x426",
                $"/Videos/{videoname}/240p.m3u8",
                "#EXT-X-STREAM-INF:BANDWIDTH=420000,RESOLUTION=360x640",
                $"/Videos/{videoname}/360p.m3u8",
                "#EXT-X-STREAM-INF:BANDWIDTH=680000,RESOLUTION=480x842",
                $"/Videos/{videoname}/480p.m3u8",
                "#EXT-X-STREAM-INF:BANDWIDTH=1256000,RESOLUTION=720x1280",
                $"/Videos/{videoname}/720p.m3u8",
                "#EXT-X-STREAM-INF:BANDWIDTH=2000000,RESOLUTION=1080x1920",
                $"/Videos/{videoname}/1080p.m3u8"
                ];

        File.WriteAllLines(path, line);
        return Task.CompletedTask;

    }

    private Task RunCommand(string command)
    {
        var tcs = new TaskCompletionSource<bool>();
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "foot" : "cmd.exe",
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? $"{command}" : $"/c \"{command}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true
        };

        process.Exited += (sender, args) =>
        {
            tcs.SetResult(true);
            process.Dispose();
        };

        process.Start();
        return tcs.Task;
    }


}