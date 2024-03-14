import '/imports.dart';
import 'package:http/http.dart' as http;

class CreateVideoPage extends StatefulWidget {
  const CreateVideoPage({super.key, this.title});

  final String? title;

  @override
  State<CreateVideoPage> createState() => _CreateVideoPageState();
}

class _CreateVideoPageState extends State<CreateVideoPage> {
  List<XFile>? _mediaFileList;
  late XFile videoFile;

  void _setImageFileListFromFile(XFile? value) {
    _mediaFileList = value == null ? null : <XFile>[value];
  }

  dynamic _pickImageError;
  bool isVideo = false;

  VideoPlayerController? _controller;
  VideoPlayerController? _toBeDisposed;
  String? _retrieveDataError;

  final ImagePicker _picker = ImagePicker();
  final TextEditingController maxWidthController = TextEditingController();
  final TextEditingController maxHeightController = TextEditingController();
  final TextEditingController qualityController = TextEditingController();

  Future<void> _playVideo(XFile? file) async {
    if (file != null && mounted) {
      await _disposeVideoController();

      late VideoPlayerController controller;

      if (kIsWeb) {
        controller = VideoPlayerController.networkUrl(Uri.parse(file.path));
      } else {
        controller = VideoPlayerController.file(File(file.path));
      }

      _controller = controller;
      // In web, most browsers won't honor a programmatic call to .play
      // if the video has a sound track (and is not muted).
      // Mute the video so it auto-plays in web!
      // This is not needed if the call to .play is the result of user
      // interaction (clicking on a "play" button, for example).
      const double volume = kIsWeb ? 0.0 : 1.0;
      await controller.setVolume(volume);
      await controller.initialize();
      await controller.setLooping(true);
      await controller.play();

      setState(() {});
    }
  }

  Future<void> _onImageButtonPressed(
    ImageSource source, {
    required BuildContext context,
  }) async {
    if (_controller != null) {
      await _controller!.setVolume(0.0);
    }

    if (context.mounted) {
      if (isVideo) {
        final XFile? file = await _picker.pickVideo(source: source);
        if (file != null) {
          videoFile = file;
        }

        await _playVideo(file);
      }
    }
  }

  @override
  void deactivate() {
    if (_controller != null) {
      _controller!.setVolume(0.0);
      _controller!.pause();
    }

    super.deactivate();
  }

  @override
  void dispose() {
    _disposeVideoController();
    maxWidthController.dispose();
    maxHeightController.dispose();
    qualityController.dispose();
    super.dispose();
  }

  Future<void> _disposeVideoController() async {
    if (_toBeDisposed != null) {
      await _toBeDisposed!.dispose();
    }

    _toBeDisposed = _controller;
    _controller = null;
  }

  Widget _previewVideo() {
    final Text? retrieveError = _getRetrieveErrorWidget();
    if (retrieveError != null) {
      return retrieveError;
    }

    if (_controller == null) {
      return const Text(
        'You have not yet picked a video',
        textAlign: TextAlign.center,
      );
    }

    return Padding(
      padding: const EdgeInsets.all(10.0),
      child: AspectRatioVideo(_controller),
    );
  }

  Widget _previewImages() {
    final Text? retrieveError = _getRetrieveErrorWidget();
    if (retrieveError != null) {
      return retrieveError;
    }

    if (_mediaFileList != null) {
      return Semantics(
        label: 'picked video',
        child: ListView.builder(
          key: UniqueKey(),
          itemBuilder: (BuildContext context, int index) {
            final String? mime = lookupMimeType(_mediaFileList![index].path);

            // Why network for web?
            // See https://pub.dev/packages/image_picker_for_web#limitations-on-the-web-platform
            return Semantics(
              label: 'picked video',
              child: kIsWeb
                  ? Image.network(_mediaFileList![index].path)
                  : (mime == null || mime.startsWith('image/')
                      ? Image.file(
                          File(_mediaFileList![index].path),
                          errorBuilder: (BuildContext context, Object error, StackTrace? stackTrace) {
                            return const Center(child: Text('This image type is not supported'));
                          },
                        )
                      : _buildInlineVideoPlayer(index)),
            );
          },
          itemCount: _mediaFileList!.length,
        ),
      );
    } else if (_pickImageError != null) {
      return Text(
        'Pick video error: $_pickImageError',
        textAlign: TextAlign.center,
      );
    } else {
      return const Text(
        'You have not yet picked a video.',
        textAlign: TextAlign.center,
      );
    }
  }

  Widget _buildInlineVideoPlayer(int index) {
    final VideoPlayerController controller = VideoPlayerController.file(File(_mediaFileList![index].path));
    const double volume = kIsWeb ? 0.0 : 1.0;
    controller.setVolume(volume);
    controller.initialize();
    controller.setLooping(true);
    controller.play();
    return Center(child: AspectRatioVideo(controller));
  }

  Widget _handlePreview() {
    if (isVideo) {
      return _previewVideo();
    } else {
      return _previewImages();
    }
  }

  Future<void> retrieveLostData() async {
    final LostDataResponse response = await _picker.retrieveLostData();
    if (response.isEmpty) {
      return;
    }

    if (response.file != null) {
      if (response.type == RetrieveType.video) {
        isVideo = true;
        await _playVideo(response.file);
      } else {
        isVideo = false;

        setState(() {
          if (response.files == null) {
            _setImageFileListFromFile(response.file);
          } else {
            _mediaFileList = response.files;
          }
        });
      }
    } else {
      _retrieveDataError = response.exception!.code;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: !kIsWeb && defaultTargetPlatform == TargetPlatform.android
            ? FutureBuilder<void>(
                future: retrieveLostData(),
                builder: (BuildContext context, AsyncSnapshot<void> snapshot) {
                  switch (snapshot.connectionState) {
                    case ConnectionState.none:
                    case ConnectionState.waiting:
                      return const Text(
                        'You have not yet picked a video.',
                        textAlign: TextAlign.center,
                      );
                    case ConnectionState.done:
                      return _handlePreview();
                    case ConnectionState.active:
                      if (snapshot.hasError) {
                        return Text(
                          'Pick video error: ${snapshot.error}}',
                          textAlign: TextAlign.center,
                        );
                      } else {
                        return const Text(
                          'You have not yet picked a video.',
                          textAlign: TextAlign.center,
                        );
                      }
                  }
                },
              )
            : _handlePreview(),
      ),
      floatingActionButton: _controller == null
          ? Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: <Widget>[
                if (_picker.supportsImageSource(ImageSource.camera))
                  Padding(
                    padding: const EdgeInsets.only(left: 16.0),
                    child: FloatingActionButton(
                      backgroundColor: Colors.red,
                      onPressed: () {
                        isVideo = true;
                        _onImageButtonPressed(ImageSource.camera, context: context);
                      },
                      heroTag: 'video1',
                      tooltip: 'Take a Video',
                      child: const Icon(Icons.videocam),
                    ),
                  ),
                if (_picker.supportsImageSource(ImageSource.camera))
                  Padding(
                    padding: const EdgeInsets.only(left: 16.0),
                    child: FloatingActionButton(
                      backgroundColor: Colors.red,
                      onPressed: () {
                        isVideo = true;
                        _onImageButtonPressed(ImageSource.gallery, context: context);
                      },
                      heroTag: 'video0',
                      tooltip: 'Pick Video from gallery',
                      child: const Icon(Icons.video_library),
                    ),
                  ),
              ],
            )
          : _controller!.value.isPlaying
              ? Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: <Widget>[
                    Padding(
                      padding: const EdgeInsets.only(left: 16.0),
                      child: FloatingActionButton(
                        backgroundColor: Colors.red,
                        onPressed: () {
                          try {
                            _controller?.pause();
                          } catch (e) {
                            debugPrint(e.toString());
                          }

                          setState(() {});
                        },
                        heroTag: 'video2',
                        tooltip: 'Remove Video',
                        child: const Icon(Icons.clear),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(left: 16.0),
                      child: FloatingActionButton(
                        backgroundColor: Colors.red,
                        onPressed: () async {
                          File file = File(videoFile.path);

                          // Create a multipart request
                          var request = http.MultipartRequest(
                            'POST',
                            Uri.parse('http://192.168.1.45:5050/TranscodeVideo/AddVideo'),
                          );

                          // Add the file to the request
                          request.files.add(
                            await http.MultipartFile.fromPath(
                              'file',
                              file.path,
                            ),
                          );

                          // Send the request
                          var response = await request.send();

                          if (response.statusCode == 200) {
                            debugPrint('File uploaded successfully');
                          } else {
                            debugPrint('Failed to upload file');
                          }

                          try {
                            _controller?.pause();
                          } catch (e) {
                            debugPrint(e.toString());
                          }

                          setState(() {});
                        },
                        heroTag: 'video2',
                        tooltip: 'Upload Video',
                        child: const Icon(Icons.upload),
                      ),
                    ),
                  ],
                )
              : Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: <Widget>[
                    if (_picker.supportsImageSource(ImageSource.camera))
                      Padding(
                        padding: const EdgeInsets.only(left: 16.0),
                        child: FloatingActionButton(
                          backgroundColor: Colors.red,
                          onPressed: () {
                            isVideo = true;
                            _onImageButtonPressed(ImageSource.camera, context: context);
                          },
                          heroTag: 'video1',
                          tooltip: 'Take a Video',
                          child: const Icon(Icons.videocam),
                        ),
                      ),
                    if (_picker.supportsImageSource(ImageSource.camera))
                      Padding(
                        padding: const EdgeInsets.only(left: 16.0),
                        child: FloatingActionButton(
                          backgroundColor: Colors.red,
                          onPressed: () {
                            isVideo = true;
                            _onImageButtonPressed(ImageSource.gallery, context: context);
                          },
                          heroTag: 'video0',
                          tooltip: 'Pick Video from gallery',
                          child: const Icon(Icons.video_library),
                        ),
                      ),
                  ],
                ),
    );
  }

  Text? _getRetrieveErrorWidget() {
    if (_retrieveDataError != null) {
      final Text result = Text(_retrieveDataError!);
      _retrieveDataError = null;
      return result;
    }
    return null;
  }
}

typedef OnPickImageCallback = void Function(double? maxWidth, double? maxHeight, int? quality);

class AspectRatioVideo extends StatefulWidget {
  const AspectRatioVideo(this.controller, {super.key});

  final VideoPlayerController? controller;

  @override
  AspectRatioVideoState createState() => AspectRatioVideoState();
}

class AspectRatioVideoState extends State<AspectRatioVideo> {
  VideoPlayerController? get controller => widget.controller;
  bool initialized = false;

  void _onVideoControllerUpdate() {
    if (!mounted) {
      return;
    }
    if (initialized != controller!.value.isInitialized) {
      initialized = controller!.value.isInitialized;
      setState(() {});
    }
  }

  @override
  void initState() {
    super.initState();
    controller!.addListener(_onVideoControllerUpdate);
  }

  @override
  void dispose() {
    controller!.removeListener(_onVideoControllerUpdate);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (initialized) {
      return Center(
        child: AspectRatio(
          aspectRatio: controller!.value.aspectRatio,
          child: VideoPlayer(controller!),
        ),
      );
    } else {
      return Container();
    }
  }
}
