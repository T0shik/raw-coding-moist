import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/models/Response.dart';
import 'package:moist_store/models/Schema.dart';
import 'package:moist_store/models/Shop.dart';
import 'package:moist_store/pages/profile/services/api_provider.dart';
import 'package:provider/provider.dart';

class DashboardWidget extends StatelessWidget {

  @override
  Widget build(BuildContext context) {
    var api = context.select((ApiProvider api) => api);
    var shopProfile = context.select((Shop shopProfile) => shopProfile);

    return Scaffold(
      appBar: AppBar(
        title: Text(shopProfile.name),
      ),
      body: Center(
        child: ListView(
          children: <Widget>[
            MaterialButton(
              child: Text("Call Api"),
              onPressed: () => {},
            ),
            MaterialButton(
              child: Text("Create Profile"),
              onPressed: () => Navigator.pushNamed(context, "/setup"),
            ),
            FutureBuilder<CollectionResponse<Schema>>(
              future: api.getSchemas(),
              builder: (BuildContext context,
                  AsyncSnapshot<CollectionResponse<Schema>> snapshot) {
                print("-- snap --");
                print(snapshot.hasData);
                print(snapshot.hasError);
                print(snapshot.error);

                if (snapshot.hasData) {
                  final response = snapshot.data;
                  if (response.error) {
                    return Text(response.message);
                  } else {

                    print(response.data);

                    if(response.data.length == 0){
                      return Text("EMPTY");
                    }

                    return Table(
                      children: response.data.map((s) =>
                        TableRow(
                            children:[
                              Text(s.title),
                              Text(s.description),
                            ]
                        )
                      )
                    );
                  }
                }

                return Scaffold(
                  body: Center(
                    child: CircularProgressIndicator(),
                  ),
                );
              },
            )

          ],
        ),
      ),
    );
  }

}