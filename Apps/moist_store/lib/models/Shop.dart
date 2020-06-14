class Shop {
  int id;
  String name;
  String description;

  Shop(this.id, this.name, this.description);

  factory Shop.fromJson(dynamic json) {
    return Shop(json['id'], json['name'], json['description']);
  }
}