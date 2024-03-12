import '/imports.dart'; // Import necessary packages and dependencies

class VideoWidget extends StatefulWidget {
  final List<Uri> videoUrls; // List of video URLs

  const VideoWidget({super.key, required this.videoUrls});

  @override
  VideoWidgetState createState() => VideoWidgetState();
}

class VideoWidgetState extends State<VideoWidget> {
  late List<VideoPlayerController> _controllers; // List of video player controllers
  late List<Future<void>> _initializeVideoPlayerFutures; // Future list for initializing video player controllers
  late int _currentVideoIndex; // Index of the currently displayed video
  late bool _isPlaying; // Flag to track if video is currently playing
  double _sliderValue = 0.0; // Value of the slider for video seek position
  String _currentVideoPosition = '00:00 / 00:00';

  @override
  void initState() {
    super.initState();

    // Initialize video player controllers for each video URL
    _controllers = widget.videoUrls
        .map(
          (url) => VideoPlayerController.networkUrl(
            url,
          ),
        )
        .toList();

    // Initialize future list for initializing video player controllers
    _initializeVideoPlayerFutures = List<Future<void>>.generate(
      widget.videoUrls.length,
      (index) => _controllers[index].initialize(),
    );

    // Initialize initial values
    _currentVideoIndex = 0;
    _isPlaying = false;

    // Set looping for all video player controllers
    for (var controller in _controllers) {
      controller.setLooping(true);
    }

    // Start playing the first video
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

    // Dispose all video player controllers
    for (var controller in _controllers) {
      controller.dispose();
    }
  }

  // Toggle play/pause state of the video
  void _togglePlay() {
    final Duration currentPosition = _controllers[_currentVideoIndex].value.position;
    final Duration duration = _controllers[_currentVideoIndex].value.duration;

    // Calculate currentPositionInVideo in "mm:ss" format
    String getFormattedTime(Duration duration) {
      String minutes = (duration.inMinutes % 60).toString().padLeft(2, '0');
      String seconds = (duration.inSeconds % 60).toString().padLeft(2, '0');
      return "$minutes:$seconds";
    }

    // Format currentPositionInVideo and videoDuration
    String currentPositionInVideo = getFormattedTime(currentPosition);
    String videoDuration = getFormattedTime(duration);

    setState(() {
      if (_isPlaying) {
        _controllers[_currentVideoIndex].pause();

        _sliderValue = currentPosition.inMilliseconds.toDouble() / duration.inMilliseconds.toDouble();

        _currentVideoPosition = '$currentPositionInVideo / $videoDuration';
      } else {
        _controllers[_currentVideoIndex].play();
      }

      _isPlaying = !_isPlaying;
    });
  }

  // Callback function when slider value changes
  void _onSliderChanged(double value) {
    final Duration duration = _controllers[_currentVideoIndex].value.duration;

    // Calculate currentPositionInVideo in "mm:ss" format
    String getFormattedTime(Duration duration) {
      String minutes = (duration.inMinutes % 60).toString().padLeft(2, '0');
      String seconds = (duration.inSeconds % 60).toString().padLeft(2, '0');
      return "$minutes:$seconds";
    }

    // Format videoDuration
    String videoDuration = getFormattedTime(duration);

    // Calculate the slider value in "mm:ss" format
    double sliderValueInMs = _sliderValue * duration.inMilliseconds;
    Duration sliderValueDuration = Duration(milliseconds: sliderValueInMs.round());

    String sliderPosition = getFormattedTime(sliderValueDuration);

    setState(() {
      _sliderValue = value;
      _currentVideoPosition = '$sliderPosition / $videoDuration';
    });
  }

  // Callback function when slider value change ends
  void _onSliderChangeEnd(double value) {
    setState(() {
      _sliderValue = value;
    });

    final Duration duration = _controllers[_currentVideoIndex].value.duration;

    // Seek to the specified position in the video
    _controllers[_currentVideoIndex].seekTo(Duration(milliseconds: (_sliderValue * duration.inMilliseconds).round()));

    // Resume playing the video
    _controllers[_currentVideoIndex].play();
    _isPlaying = true;
  }

  // Callback function for liking a video
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
                    VideoPlayer(_controllers[index]), // Display the video player

                    if (!_isPlaying && _currentVideoIndex == index)
                      const Icon(Icons.play_arrow,
                          size: 72.0, color: Colors.white), // Show play icon if video is paused
                  ],
                ),
              ),
              if (!_isPlaying && _currentVideoIndex == index)
                Column(
                  children: [
                    Padding(
                      padding: const EdgeInsets.fromLTRB(16, 4, 16, 4),
                      child: Text(_currentVideoPosition),
                    ),
                    Padding(
                      padding: const EdgeInsets.fromLTRB(16, 4, 16, 4),
                      // child: VideoProgressIndicator(
                      //   _controllers[index],
                      //   allowScrubbing: true,
                      //   padding: const EdgeInsets.all(0),
                      // ),
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
