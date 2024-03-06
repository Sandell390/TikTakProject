import 'imports.dart';

void main() {
  // Ensure the widgets have been initialized before starting the app
  WidgetsFlutterBinding.ensureInitialized();

  // Start the app
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key});

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
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
                    onPressed: () {
                      // Handle like button press
                    },
                  ),
                  IconButton(
                    icon: const Icon(Icons.comment),
                    onPressed: () {
                      // Handle comment button press
                    },
                  ),
                  IconButton(
                    icon: const Icon(Icons.share),
                    onPressed: () {
                      // Handle share button press
                    },
                  ),
                  IconButton(
                    icon: const Icon(Icons.more_vert),
                    onPressed: () {
                      // Handle more button press
                    },
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
              onPressed: () {
                // Handle more comments button press
              },
              child: const Text('View more comments'),
            ),
          ],
        ),
      ),
    );
  }
}
