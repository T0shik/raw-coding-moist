import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:moist_store/services/moist_client.dart';
import 'package:provider/provider.dart';

class Dashboard extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => DashState();
}

class DashState extends State<Dashboard> {

  String _response = "Empty";

  _apiCall(MoistClient client) async {
    var response = await client.test();
    setState(() => _response = response.body);
  }

  @override
  Widget build(BuildContext context) {
    var client = context.select((MoistClient client) => client);

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
              onPressed: () => _apiCall(client),
            ),
            MaterialButton(
              child: Text("Create Profile"),
              onPressed: () => Navigator.pushNamed(context, "/profile/create"),
            )
          ],
        ),
      ),
    );
  }

}