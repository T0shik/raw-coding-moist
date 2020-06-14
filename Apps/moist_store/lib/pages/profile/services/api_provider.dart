import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import 'package:moist_store/models/Response.dart';
import 'package:moist_store/models/Schema.dart';
import 'package:moist_store/models/Shop.dart';

class ApiProvider {
  http.Client _client;
  Map<String, String> _headers;
  static const String _url = "http://192.168.1.104:8005/api";

  ApiProvider(String accessToken) {
    _client = http.Client();
    print("ACCESS TOKEN! $accessToken");
    _headers = {
      HttpHeaders.authorizationHeader: "Bearer $accessToken",
      HttpHeaders.contentTypeHeader: "application/json"
    };

    print(_headers);
  }

  Future<Response<bool>> userInit() => _get<bool>("/users/me/init");
  Future<Response<bool>> initUser() => _post<bool>("/users/me/init", null);

  Future<Response<Shop>> createShop(Map<String, dynamic> form) => _post<Shop>("/shops", jsonEncode(form));
  Future<Response<Shop>> getProfile() => _get<Shop>("/shops/me");
  Future<CollectionResponse<Schema>> getSchemas() => _getCollection<Schema>("/shops/me/schemas");

  Future<Response<T>> _get<T>(String url) async {
    var response = await _client.get(_endpoint(url), headers: _headers);
    return _parseResponse(response);
  }

  Future<CollectionResponse<T>> _getCollection<T>(String url) async {
    var response = await _client.get(_endpoint(url), headers: _headers);
    return _parseCollectionResponse(response);
  }

  Future<Response<T>> _post<T>(String url, String json) async {
    var response = await _client.post(_endpoint(url), headers: _headers, body: json);
    return _parseResponse(response);
  }

  Future<CollectionResponse<T>> _parseCollectionResponse<T>(http.Response response) async {
    var jsonBody = jsonDecode(response.body);
    print(jsonBody);
    if (jsonBody['error']) {}

    return CollectionResponse<T>.fromJson(jsonBody);
  }

  Future<Response<T>> _parseResponse<T>(http.Response response) async {
    var jsonBody = jsonDecode(response.body);
    print(jsonBody);
    if (jsonBody['error']) {}

    return Response<T>.fromJson(jsonBody);
  }

  static String _endpoint(String endpoint) => _url + endpoint;
}
