import '../imports.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    var mediaQueryHeight = MediaQuery.of(context).size.height;
    var mediaQueryWidth = MediaQuery.of(context).size.width;
    var appBarBottomHeight = ((mediaQueryHeight / 100) * 10);
    var sidePadding = ((mediaQueryWidth / 100) * 5);
    var iconSize = ((mediaQueryHeight / 100) * 6);

    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Container(
              height: mediaQueryHeight - appBarBottomHeight,
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
              padding: EdgeInsets.fromLTRB(sidePadding, 0, sidePadding, 0),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Container(
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: const Color.fromARGB(255, 81, 255, 0),
                        width: 1,
                      ),
                    ),
                    child: Column(
                      children: [
                        SizedBox(
                          height: iconSize,
                          child: DecoratedBox(
                            decoration: BoxDecoration(
                              border: Border.all(
                                color: const Color.fromARGB(255, 255, 0, 0),
                              ),
                            ),
                            child: IconButton(
                              icon: Icon(
                                Icons.home,
                                // Icons.home_outlined,
                                color: Colors.white70,
                                size: iconSize,
                              ),
                              onPressed: () => _showSnackBar(context, 'Home button pressed'),
                            ),
                          ),
                        ),
                        Container(
                          decoration: BoxDecoration(
                            border: Border.all(
                              color: const Color.fromARGB(255, 255, 196, 0),
                              width: 1,
                            ),
                          ),
                          child: const Text(
                            "Start",
                          ),
                        ),
                      ],
                    ),
                  ),
                  Container(
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: const Color.fromARGB(255, 81, 255, 0),
                        width: 1,
                      ),
                    ),
                    child: Column(
                      children: [
                        SizedBox(
                          height: iconSize,
                          child: DecoratedBox(
                            decoration: BoxDecoration(
                              border: Border.all(
                                color: const Color.fromARGB(255, 255, 0, 0),
                              ),
                            ),
                            child: IconButton(
                              icon: Icon(
                                // Icons.add_circle,
                                Icons.add_circle_outline,
                                color: Colors.white70,
                                size: iconSize,
                              ),
                              onPressed: () => _showSnackBar(context, 'Add button pressed'),
                            ),
                          ),
                        ),
                        Container(
                          decoration: BoxDecoration(
                            border: Border.all(
                              color: const Color.fromARGB(255, 255, 196, 0),
                              width: 1,
                            ),
                          ),
                          child: const Text(
                            "Create",
                          ),
                        ),
                      ],
                    ),
                  ),
                  Container(
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: const Color.fromARGB(255, 81, 255, 0),
                        width: 1,
                      ),
                    ),
                    child: Column(
                      children: [
                        SizedBox(
                          height: iconSize,
                          child: DecoratedBox(
                            decoration: BoxDecoration(
                              border: Border.all(
                                color: const Color.fromARGB(255, 255, 0, 0),
                              ),
                            ),
                            child: IconButton(
                              icon: Icon(
                                // Icons.person,
                                Icons.person_outline,
                                color: Colors.white70,
                                size: iconSize,
                              ),
                              onPressed: () => _showSnackBar(context, 'Person button pressed'),
                            ),
                          ),
                        ),
                        Container(
                          decoration: BoxDecoration(
                            border: Border.all(
                              color: const Color.fromARGB(255, 255, 196, 0),
                              width: 1,
                            ),
                          ),
                          child: const Text(
                            "Profile",
                          ),
                        ),
                      ],
                    ),
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

void _showSnackBar(BuildContext context, String text) {
  ScaffoldMessenger.of(context)
    ..removeCurrentSnackBar()
    ..showSnackBar(
      SnackBar(
        content: Text(text),
      ),
    );
}
