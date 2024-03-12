import '/imports.dart';
import 'package:http/http.dart' as http;

// Define the base URL for the API
const String baseApiUrl = 'http://192.168.1.45:5050';

// Function to fetch video list
Future<List<VideoModel>?> fetchVideoListAsync() async {
  try {
    final response = await http.get(Uri.parse('$baseApiUrl/TranscodeVideo/GetVideoList'));

    if (response.statusCode == 200) {
      var jsonResponse = jsonDecode(response.body);

      List<VideoModel> videoModels = <VideoModel>[];

      for (var videoModelObj in jsonResponse) {
        videoModels.add(VideoModel.fromJson(videoModelObj));
      }

      for (var video in videoModels) {
        video.indexUri = '$baseApiUrl/${video.serverPath}/${video.name}/index.m3u8';
        debugPrint('Downlaod here: ${video.indexUri}');
      }

      return videoModels;
    } else {
      throw Exception('Failed to load data');
    }
  } catch (e) {
    debugPrint(e.toString());
    return null;
  }
}
