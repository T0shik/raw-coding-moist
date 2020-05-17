import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/services/authentication_provider.dart';
import 'package:moist_store/services/moist_client.dart';
import 'package:provider/provider.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({Key key}) : super(key: key);

  void _auth(BuildContext context) async {
    var auth = context.read<AuthenticationProvider>();
    var api = context.read<MoistClient>();

    var result = await auth.authenticate();

    if(result){
      var result = await api.initShop();
      print("-- Response after authentication --");
      print(result.body);
    }

    Navigator.pushReplacementNamed(context, '/dashboard');
  }

  @override
  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(
        title: Text("Login Page"),
      ),
      body: Center(
        child: RaisedButton(
          onPressed: () => _auth(context),
          child: Text('Login'),
        ),
      ),
    );
  }
}
