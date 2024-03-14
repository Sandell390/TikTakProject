import '/imports.dart';

class VideoPage extends StatelessWidget {
  const VideoPage({super.key});

  @override
  Widget build(BuildContext context) {
    VideoApiProvider videoListProvider = context.watch<VideoApiProvider>();
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
