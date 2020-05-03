import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';

import '../main.dart';

class Dashboard extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => DashState();
}

class DashState extends State<Dashboard> {

  String _response = "Empty";

  _apiCall(String accessToken) async {
    var client = http.Client();

    var response = await client.get(
      "http://192.168.1.107:8005/home/secure",
      headers: {HttpHeaders.authorizationHeader: "Bearer $accessToken"},
    );

    setState(() => _response = response.body);
  }

  @override
  Widget build(BuildContext context) {
    var provider = Provider.of<AuthenticationProvider>(context);

    return Scaffold(
      appBar: AppBar(
        title: Text('Dashboard'),
      ),
      body: Center(
        child: ListView(
          children: <Widget>[
            Text(_response),
            MaterialButton(
              child: Text("Call Api"),
              onPressed: () => _apiCall(provider.getAccessToken()),
            )
          ],
        ),
      ),
    );
  }

}