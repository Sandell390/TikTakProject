class VideoModel {
  final String name;
  final String serverPath;
  String indexUri;

  VideoModel({required this.name, required this.serverPath, required this.indexUri});

  factory VideoModel.fromJson(Map<String, dynamic> json) {
    return VideoModel(name: json['name'], serverPath: json['serverPath'], indexUri: '');
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};

    data['name'] = name;
    data['serverPath'] = serverPath;
    data['indexUri'] = indexUri;

    return data;
  }
}
