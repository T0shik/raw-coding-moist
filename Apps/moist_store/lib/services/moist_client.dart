import 'dart:io';

import 'package:http/http.dart' as http;
import 'authentication_provider.dart';

class MoistClient {
  http.Client _client;
  Map<String, String> _headers;
  AuthenticationProvider _auth;

  MoistClient({AuthenticationProvider auth}) {
    _client = http.Client();
    _auth = auth;
  }

  _init(){
    var accessToken = _auth.getAccessToken();
    _headers = {HttpHeaders.authorizationHeader: "Bearer $accessToken"};
  }

  test() {
    return _client.get("http://192.168.1.100:8005/home/secure",
        headers: _headers);
  }

  initShop() {
    _init();
    return _client.post("http://192.168.1.100:8005/api/shops",
        headers: _headers);
  }

  getProfile() {
    return _client.get("http://192.168.1.100:8005/api/shops",
        headers: _headers);
  }
}
