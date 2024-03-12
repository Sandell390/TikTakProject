using System.Text.Json.Serialization;

namespace TikTakWebAPI.Models
{
    public class VideoModel
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("serverPath")]
        public string? ServerPath { get; set; }

        public VideoModel()
        { }
        public VideoModel(string name, string serverPath)
        {
            Name = name;
            ServerPath = serverPath;
        }
    }
}
