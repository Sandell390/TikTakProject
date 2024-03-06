import '../imports.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key, required this.title});
  final String title;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: const Text('TikTak'),
      ),
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Container(
              height: 200, // Adjust height as needed
              color: Colors.grey[300], // Placeholder for video
              child: const Center(
                child: Icon(
                  Icons.play_arrow,
                  size: 50,
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  const CircleAvatar(
                    radius: 25,
                    backgroundColor: Color.fromRGBO(149, 30, 162, 1.0),
                    // Placeholder for user profile image
                    // You can replace this with an actual image
                    child: Icon(Icons.person),
                  ),
                  const Expanded(
                    child: Padding(
                      padding: EdgeInsets.symmetric(horizontal: 8.0),
                      child: Text(
                        'User Name',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                  ),
                  IconButton(
                    icon: const Icon(Icons.favorite_border),
                    onPressed: () =>
                        _showSnackBar(context, 'Like button pressed'),
                  ),
                  IconButton(
                    icon: const Icon(Icons.comment),
                    onPressed: () =>
                        _showSnackBar(context, 'Comment button pressed'),
                  ),
                  IconButton(
                    icon: const Icon(Icons.share),
                    onPressed: () =>
                        _showSnackBar(context, 'Share button pressed'),
                  ),
                  IconButton(
                    icon: const Icon(Icons.more_vert),
                    onPressed: () =>
                        _showSnackBar(context, 'More button pressed'),
                  ),
                ],
              ),
            ),
            // Placeholder for caption
            const Padding(
              padding: EdgeInsets.all(8.0),
              child: Text(
                'Caption text',
                style: TextStyle(
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            // Placeholder for comments
            const ListTile(
              leading: CircleAvatar(
                // Placeholder for commenter profile image
                // You can replace this with an actual image
                child: Icon(Icons.person),
              ),
              title: Text('Commenter Name'),
              subtitle: Text('Comment text'),
            ),
            // Placeholder for more comments
            const ListTile(
              leading: CircleAvatar(
                // Placeholder for commenter profile image
                // You can replace this with an actual image
                child: Icon(Icons.person),
              ),
              title: Text('Commenter Name'),
              subtitle: Text('Comment text'),
            ),
            // Placeholder for more comments button
            TextButton(
              onPressed: () =>
                  _showSnackBar(context, 'More comments button pressed'),
              child: const Text('View more comments'),
            ),
          ],
        ),
      ),
    );
  }
}

void _showSnackBar(BuildContext context, String text) {
  ScaffoldMessenger.of(context)
    ..removeCurrentSnackBar()
    ..showSnackBar(
      SnackBar(
        content: Text(text),
      ),
    );
}
