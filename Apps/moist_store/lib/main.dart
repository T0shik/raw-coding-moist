import 'package:flutter/material.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:http/http.dart' as http;
import 'dart:io';

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
  Credential cred;

  _auth() async {
    var uri = new Uri(host: "192.168.1.107", port: 8004, scheme: "http");
    var clientId = "flutter_shop";
    var issuer = await Issuer.discover(uri);
    var client = new Client(issuer, clientId);

    urlLauncher(String url) async {
      if (await canLaunch(url)) {
        await launch(url, forceWebView: true);
      } else {
        throw 'Could not launch $url';
      }
    }

    var authenticator = new Authenticator(client,
        scopes: ['user-api'], port: 4000, urlLancher: urlLauncher);

    cred = await authenticator.authorize();
    closeWebView();

    var userInfo = await cred.getUserInfo();

    return userInfo;
  }

  _apiCall() async {
    var client = http.Client();

    var rezi = await cred.getTokenResponse();
    
    var accessToken = rezi.accessToken;
    
    var response = await client.get(
      "http://10.0.2.2:5001/home/secure", 
      headers: {HttpHeaders.authorizationHeader: "Bearer $accessToken"},);
    var a = response.body;
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
            OutlineButton(child: Text("Oi"), onPressed: _apiCall),
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
