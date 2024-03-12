import '/imports.dart';

class VideoScreen extends StatelessWidget {
  final List<Uri> videoUrls = [
    Uri.parse('https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8'),
    Uri.parse('http://192.168.1.45:5050/TranscodeVideo/stream/ebb5736c-1023-40eb-8531-6906b773cbad'),
    Uri.parse(
        'https://media.istockphoto.com/id/1464801423/fr/vid%C3%A9o/ralenti-beaux-chevaux-courant-dans-loc%C3%A9an-ensoleill%C3%A9-au-lever-du-soleil.mp4?s=mp4-480x480-is&k=20&c=dMnHjFkUFlm_MOkRAxFfnTWUeDnCQW5-ABERKGQEY6c='),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+1'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+2'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+3'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+4'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+5'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+6'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+7'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+8'),
    Uri.parse('https://placehold.co/1080x1920.mp4?text=Video+9'),
  ];

  VideoScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return VideoWidget(videoUrls: videoUrls);
  }
}
