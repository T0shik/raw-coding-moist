class Schema {
  int id;
  String title;
  String description;
  SchemaState state;

  Schema(this.id, this.title, this.description, this.state);

  factory Schema.fromJson(dynamic json) {

    return Schema(
        json['id'],
        json['title'],
        json['description'],
        _stateFactory(json['state'])
    );
  }

  static SchemaState _stateFactory(int state){
    if(state == 0) return SchemaState.Started;
    if(state == 1) return SchemaState.Active;
    if(state == 2) return SchemaState.Closed;

    throw Exception("invalid schema state: $state");
  }
}

enum SchemaState {
  Started,
  Active,
  Closed,
}