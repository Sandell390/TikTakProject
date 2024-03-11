import '/imports.dart';

class VideoWidget extends StatefulWidget {
  final List<Uri> videoUrls;

  const VideoWidget({super.key, required this.videoUrls});

  @override
  VideoWidgetState createState() => VideoWidgetState();
}

class VideoWidgetState extends State<VideoWidget> {
  late List<VideoPlayerController> _controllers;
  late List<Future<void>> _initializeVideoPlayerFutures;
  late int _currentVideoIndex;
  late bool _isPlaying;

  @override
  void initState() {
    super.initState();

    _controllers = widget.videoUrls.map((url) => VideoPlayerController.networkUrl(url)).toList();

    _initializeVideoPlayerFutures = List<Future<void>>.generate(
      widget.videoUrls.length,
      (index) => _controllers[index].initialize(),
    );

    _currentVideoIndex = 0;
    _isPlaying = false;

    for (var controller in _controllers) {
      controller.setLooping(true);
    }

    _initializeVideoPlayerFutures[_currentVideoIndex].then((_) {
      setState(() {
        _isPlaying = true;
      });

      _controllers[_currentVideoIndex].play();
    });
  }

  @override
  void dispose() {
    super.dispose();

    for (var controller in _controllers) {
      controller.dispose();
    }
  }

  void _togglePlay() {
    setState(() {
      if (_isPlaying) {
        _controllers[_currentVideoIndex].pause();
      } else {
        _controllers[_currentVideoIndex].play();
      }
      _isPlaying = !_isPlaying;
    });
  }

  void _likeVideo() {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(_controllers[_currentVideoIndex].dataSource),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: _togglePlay,
      onDoubleTap: _likeVideo,
      child: PageView.builder(
        controller: PageController(),
        scrollDirection: Axis.vertical,
        itemCount: widget.videoUrls.length,
        itemBuilder: (context, index) {
          return Stack(
            alignment: Alignment.center,
            children: [
              VideoPlayer(_controllers[index]),
              if (!_isPlaying && _currentVideoIndex == index)
                const Icon(Icons.play_arrow, size: 72.0, color: Colors.white),
            ],
          );
        },
        onPageChanged: (index) {
          _controllers[_currentVideoIndex].pause(); // Pause previous video

          setState(() {
            _currentVideoIndex = index; // Update current video index
            _isPlaying = true; // Reset play state
          });

          _controllers[_currentVideoIndex].play(); // Play new video
        },
      ),
    );
  }
}
