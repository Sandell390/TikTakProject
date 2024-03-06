import '../imports.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    const appBarBottomHeight = 75.0;
    const iconSize = 30.0;
    const sidePadding = 10.0;

    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Container(
              height: MediaQuery.of(context).size.height - appBarBottomHeight,
              color: const Color.fromARGB(255, 190, 190, 190), // Placeholder for video
              child: Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    IconButton(
                      icon: const Icon(
                        Icons.play_arrow,
                        size: 50,
                        color: Color.fromARGB(100, 255, 255, 255),
                      ),
                      onPressed: () => _showSnackBar(context, 'Play button pressed'),
                    ),
                    const Text("Press to play"),
                  ],
                ),
              ),
            ),
            Container(
              color: const Color.fromARGB(255, 0, 0, 0),
              height: appBarBottomHeight,
              padding: const EdgeInsets.fromLTRB(sidePadding, 0, sidePadding, 0),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Column(
                    children: [
                      IconButton(
                        icon: const Icon(
                          Icons.home,
                          // Icons.home_outlined,
                          color: Colors.white70,
                          size: iconSize,
                        ),
                        onPressed: () => _showSnackBar(context, 'Home button pressed'),
                      ),
                      const Text(
                        "Start",
                        // textScaler: TextScaler.linear(.5),
                      ),
                    ],
                  ),
                  Column(
                    children: [
                      IconButton(
                        icon: const Icon(
                          // Icons.add_circle,
                          Icons.add_circle_outline,
                          color: Colors.white70,
                          size: iconSize,
                        ),
                        onPressed: () => _showSnackBar(context, 'Add button pressed'),
                      ),
                      const Text(
                        "Create",
                      ),
                    ],
                  ),
                  Column(
                    children: [
                      IconButton(
                        icon: const Icon(
                          // Icons.person,
                          Icons.person_outline,
                          color: Colors.white70,
                          size: iconSize,
                        ),
                        onPressed: () => _showSnackBar(context, 'Person button pressed'),
                      ),
                      const Text(
                        "Profile",
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
/*
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
          
*/

void _showSnackBar(BuildContext context, String text) {
  ScaffoldMessenger.of(context)
    ..removeCurrentSnackBar()
    ..showSnackBar(
      SnackBar(
        content: Text(text),
      ),
    );
}
