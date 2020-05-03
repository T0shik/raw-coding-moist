import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/main.dart';
import 'package:openid_client/openid_client.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:provider/provider.dart';
import 'package:url_launcher/url_launcher.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({Key key}) : super(key: key);

  void _auth(BuildContext context, AuthenticationProvider provider) async {
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

    var cred = await authenticator.authorize();
    closeWebView();
    await provider.setCredentials(cred);
    Navigator.pushNamed(context, '/dashboard');
  }

  @override
  Widget build(BuildContext context) {
    var provider = Provider.of<AuthenticationProvider>(context);

    return Scaffold(
      appBar: AppBar(
        title: Text("Login Page"),
      ),
      body: Center(
        child: RaisedButton(
          onPressed: () => _auth(context, provider),
          child: Text('Login'),
        ),
      ),
    );
  }
}
