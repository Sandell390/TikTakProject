import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(),
    );
  }
}

class MyHomePage extends StatefulWidget {
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('TikTak'),
      ),
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Container(
              height: 200, // Adjust height as needed
              color: Colors.grey[300], // Placeholder for video
              child: Center(
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
                  CircleAvatar(
                    radius: 25,
                    backgroundColor: Color.fromRGBO(149, 30, 162, 1.0),
                    // Placeholder for user profile image
                    // You can replace this with an actual image
                    child: Icon(Icons.person),
                  ),
                  Expanded(
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 8.0),
                      child: Text(
                        'User Name',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                  ),
                  IconButton(
                    icon: Icon(Icons.favorite_border),
                    onPressed: () {
                      // Handle like button press
                    },
                  ),
                  IconButton(
                    icon: Icon(Icons.comment),
                    onPressed: () {
                      // Handle comment button press
                    },
                  ),
                  IconButton(
                    icon: Icon(Icons.share),
                    onPressed: () {
                      // Handle share button press
                    },
                  ),
                  IconButton(
                    icon: Icon(Icons.more_vert),
                    onPressed: () {
                      // Handle more button press
                    },
                  ),
                ],
              ),
            ),
            // Placeholder for caption
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Text(
                'Caption text',
                style: TextStyle(
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            // Placeholder for comments
            ListTile(
              leading: CircleAvatar(
                // Placeholder for commenter profile image
                // You can replace this with an actual image
                child: Icon(Icons.person),
              ),
              title: Text('Commenter Name'),
              subtitle: Text('Comment text'),
            ),
            // Placeholder for more comments
            ListTile(
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
              child: Text('View more comments'),
            ),
          ],
        ),
      ),
    );
  }
}
