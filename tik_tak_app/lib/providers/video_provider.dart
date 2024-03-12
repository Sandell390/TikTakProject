import '/imports.dart';

class VideoProvider extends ChangeNotifier {
  late List<VideoModel?> _videoModels = <VideoModel>[];
  List<VideoModel?> get videoModels => _videoModels;

  void getVideoListAsync() async {
    _videoModels = await fetchVideoListAsync() ?? <VideoModel>[];
    notifyListeners();
  }
}
