import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/services/authentication_provider.dart';
import 'package:provider/provider.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var authService = context.select((AuthenticationProvider auth) => auth);

    return Scaffold(
      appBar: AppBar(
        title: Text("Login Page"),
      ),
      body: Center(
        child: RaisedButton(
          onPressed: authService.authenticate,
          child: Text('Login'),
        ),
      ),
    );
  }
}
