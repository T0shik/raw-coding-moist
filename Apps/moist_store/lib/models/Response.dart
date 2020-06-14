import 'dart:convert';

import 'package:moist_store/models/Schema.dart';

import 'Shop.dart';

class Response<T>
{
  T data;
  String message;
  bool error;

  Response(this.data, this.message, this.error);

  factory Response.fromJson(dynamic json) {
    if(T == Shop){
      return Response(Shop.fromJson(json['data']) as T, json['message'] as String, json['error'] as bool);
    }
    return Response(null, json['message'] as String, json['error'] as bool);
  }
}

class CollectionResponse<T>
{
  List<T> data;
  String message;
  bool error;

  CollectionResponse(this.data, this.message, this.error);

  factory CollectionResponse.fromJson(dynamic json) {
    if(T == Schema){
      return CollectionResponse(_parseList<T>(json['data']), json['message'] as String, json['error'] as bool);
    }
    return CollectionResponse(null, json['message'] as String, json['error'] as bool);
  }

  static List<T> _parseList<T>(jsonData) {
    var list = jsonData as List;
    if(list.length == 0) return [];
    return list.map((x)=> Schema.fromJson(x)).toList() as List<T>;
  }

}
