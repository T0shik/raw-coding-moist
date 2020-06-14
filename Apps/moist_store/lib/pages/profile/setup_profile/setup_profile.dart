import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/pages/profile/services/shop_provider.dart';
import 'package:provider/provider.dart';

class SetupProfileWidget extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return SetupProfileWidgetState();
  }
}

class SetupProfileWidgetState extends State<SetupProfileWidget> {
  final _formKey = GlobalKey<FormState>();
  final Map<String, dynamic> _formData = {'name': null, 'description': null};

  @override
  Widget build(BuildContext context) {
    var shopProfileService = context.select((ShopProfileProvider auth) => auth);

    return Scaffold(
        appBar: AppBar(
          title: Text("Account Setup"),
        ),
        body: Form(
          key: _formKey,
          child: Padding(
            padding: EdgeInsets.fromLTRB(10, 20, 10, 0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              children: <Widget>[
                _buildShopNameField(),
                _buildShopDescriptionField(),
                _buildSubmitButton(shopProfileService),
              ],
            ),
          )
        )
    );
  }


  Widget _buildShopNameField() {
    return TextFormField(
      decoration: InputDecoration(labelText: 'Name'),
      validator: (String value) {
        if(value.isEmpty){
          return 'Shop name is required!';
        }
        return null;
      },
      onSaved: (String value) {
        _formData['name'] = value;
      },
    );
  }

  Widget _buildShopDescriptionField() {
    return TextFormField(
      decoration: InputDecoration(labelText: 'Description'),
      onSaved: (String value) {
        _formData['description'] = value;
      },
    );
  }

  Widget _buildSubmitButton(ShopProfileProvider shopProfileService) {
    return RaisedButton(
      onPressed: () => _submitForm(shopProfileService),
      child: Text('Create'),
    );
  }

  void _submitForm(ShopProfileProvider shopProfileService) async {
    print('Submitting form');
    if (_formKey.currentState.validate()) {
      _formKey.currentState.save();
      print(_formData);
      await shopProfileService.createShop(_formData);
    }
  }
}
