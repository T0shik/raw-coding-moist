import 'dart:async';
import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:moist_store/internals/section_container.dart';
import 'package:moist_store/models/User.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:shared_preferences/shared_preferences.dart';

class AuthenticationProvider {
  final String _storage_key_tokens = "storage_key_tokens";
  final _uri = new Uri(host: "192.168.1.104", port: 8004, scheme: "http");
  final _clientId = "flutter_shop";

  bool _init = false;
  Client _client;
  Credential _credential;
  TokenResponse _tokenResponse;
  StreamController<SectionContainer<User>> _controller;

  AuthenticationProvider() {
    _controller = new StreamController<SectionContainer<User>>();
  }

  Stream<SectionContainer<User>> onUserAuthenticated() => _controller.stream;

  void init() async {
    if(_init) return;
    _init = true;

    SharedPreferences prefs = await SharedPreferences.getInstance();
    try {
      if (prefs.containsKey(_storage_key_tokens)) {
        var storedToken = prefs.getString(_storage_key_tokens);
        print(storedToken);
        var json = jsonDecode(storedToken);
        var tokenResponse = TokenResponse.fromJson(json);
        var idToken = tokenResponse.idToken.toCompactSerialization();

        _client = await Client.forIdToken(idToken);
        _credential = _client.createCredential(tokenResponse);

        await _getTokens();

        _controller.add(SectionContainer.accept());

        return;
      } else {
        var issuer = await Issuer.discover(_uri);
        _client = new Client(issuer, _clientId);
      }
    } catch (e) {
      print("auth init exception: $e");
    }

    _controller.add(SectionContainer.reject());
  }

  Future<void> authenticate() async {
    urlLauncher(String url) async {
      if (await canLaunch(url)) {
        await launch(url, forceWebView: true);
      } else {
        throw 'Could not launch $url';
      }
    }

    var authenticator = new Authenticator(_client,
        scopes: ['user_api', 'offline_access'], port: 4000, urlLancher: urlLauncher);

    _credential = await authenticator.authorize();
    await _getTokens();
    _controller.add(SectionContainer.accept());

    closeWebView();
  }

  _getTokens() async {
    _tokenResponse = await _credential.getTokenResponse();
    SharedPreferences prefs = await SharedPreferences.getInstance();
    prefs.setString(_storage_key_tokens, jsonEncode(_tokenResponse));
  }

  String getAccessToken() {
    return _tokenResponse.accessToken;
  }
}
