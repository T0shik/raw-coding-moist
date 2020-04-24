import 'package:flutter/material.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:url_launcher/url_launcher.dart';


void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(title: 'Flutter Demo Home Pagex'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;

  void _incrementCounter() {
    setState(() {
      _counter++;
    });
  }

  _auth() async {
    var uri = new Uri(host: "10.0.2.2", port: 5004, scheme: "http");
    var clientId = "flutter_shop";
    var issuer = await Issuer.discover(uri);
    var client = new Client(issuer, clientId);
    
    // create a function to open a browser with an url
    urlLauncher(String url) async {
        if (await canLaunch(url)) {
          await launch(url, forceWebView: true);
        } else {
          throw 'Could not launch $url';
        }
    }
    
    // create an authenticator
    var authenticator = new Authenticator(client,
        scopes: ['user-api'],
        port: 4000, urlLancher: urlLauncher);
    
    // starts the authentication
    var c = await authenticator.authorize();
    
    // close the webview when finished
    closeWebView();
    
    // return the user info
    var userInfo = await c.getUserInfo();

    return userInfo;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              'You have pushed the button this many times:',
            ),
            Text(
              '$_counter',
              style: Theme.of(context).textTheme.display1,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _auth,
        tooltip: 'Login',
        child: Icon(Icons.account_circle),
      ),
    );
  }
}
