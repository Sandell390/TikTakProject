import '/imports.dart';

class VideoScreen extends StatelessWidget {
  const VideoScreen({super.key});

  @override
  Widget build(BuildContext context) {
    VideoProvider videoListProvider = context.watch<VideoProvider>();
    videoListProvider.getVideoListAsync();

    List<Uri> videoUrls = videoListProvider.videoModels
        .map((videoModel) => videoModel != null ? Uri.parse(videoModel.indexUri) : Uri.parse(''))
        .toList();

    if (videoUrls.isNotEmpty) {
      return VideoWidget(videoUrls: videoUrls);
    }

    return const CircularProgressIndicator();
  }
}
