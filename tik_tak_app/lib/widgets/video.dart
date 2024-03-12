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
  double _sliderValue = 0.0;

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

        final Duration currentPosition = _controllers[_currentVideoIndex].value.position;
        final Duration duration = _controllers[_currentVideoIndex].value.duration;

        _sliderValue = currentPosition.inMilliseconds.toDouble() / duration.inMilliseconds.toDouble();
      } else {
        _controllers[_currentVideoIndex].play();
      }

      _isPlaying = !_isPlaying;
    });
  }

  void _onSliderChanged(double value) {
    setState(() {
      _sliderValue = value;
    });
  }

  // Resume playing the video when the user finishes dragging the slider
  void _onSliderChangeEnd(double value) {
    setState(() {
      _sliderValue = value;
    });

    final Duration duration = _controllers[_currentVideoIndex].value.duration;

    _controllers[_currentVideoIndex].seekTo(Duration(milliseconds: (_sliderValue * duration.inMilliseconds).round()));

    _controllers[_currentVideoIndex].play();
    _isPlaying = true;
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
          return Column(
            children: [
              Expanded(
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    VideoPlayer(_controllers[index]),
                    if (!_isPlaying && _currentVideoIndex == index)
                      const Icon(Icons.play_arrow, size: 72.0, color: Colors.white),
                  ],
                ),
              ),
              if (!_isPlaying && _currentVideoIndex == index)
                Padding(
                  padding: const EdgeInsets.fromLTRB(16, 4, 16, 4),
                  child: SliderTheme(
                    data: SliderThemeData(
                      overlayShape: SliderComponentShape.noOverlay,
                      overlayColor: Colors.transparent, // Set overlay color to transparent to remove background
                      trackShape: const RoundedRectSliderTrackShape(),
                      inactiveTrackColor: Colors.grey[500], // Set the color of the track
                      activeTrackColor: Colors.grey[800], // Set the color for the active part of the track
                      thumbShape: const RoundSliderThumbShape(
                        enabledThumbRadius: 6.9, // Adjust the size of the thumb as needed
                      ),
                      thumbColor: Colors.grey[700], // Set the color for the slider thumb
                      trackHeight: 4,
                    ),
                    child: Slider(
                      value: _sliderValue,
                      onChanged: _onSliderChanged,
                      onChangeEnd: _onSliderChangeEnd,
                    ),
                  ),
                ),
            ],
          );
        },
        onPageChanged: (index) {
          _controllers[_currentVideoIndex].pause(); // Pause previous video

          setState(() {
            _currentVideoIndex = index; // Update current video index
            _isPlaying = true; // Reset play state
            _sliderValue = 0.0; // Reset slider value
          });

          _controllers[_currentVideoIndex].play(); // Play new video
        },
      ),
    );
  }
}
