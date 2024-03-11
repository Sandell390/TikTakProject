import 'imports.dart';

void main() {
  // Ensure the widgets have been initialized before starting the app
  WidgetsFlutterBinding.ensureInitialized();

  // Start the app
  SystemChrome.setPreferredOrientations([DeviceOrientation.portraitUp]).then((_) => runApp(const TikTakApp()));
}

class TikTakApp extends StatelessWidget {
  const TikTakApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
            seedColor: Colors.deepPurple,
            // Brightness is based on OS DarkMode
            // Meaning it follows the UI of the device
            brightness: MediaQuery.of(context).platformBrightness),
        useMaterial3: true,
      ),
      home: const HomePage(),
    );
  }
}
