import 'dart:async';
import 'package:moist_store/internals/section_container.dart';
import 'package:moist_store/models/Schema.dart';
import 'package:moist_store/models/Shop.dart';
import 'api_provider.dart';

class ShopProfileProvider {
  ApiProvider _api;
  StreamController<SectionContainer<Shop>> _controller;
  Shop profile;
//  List<Schema> schemas;

  ShopProfileProvider(this._api) {
    _controller = new StreamController<SectionContainer<Shop>>();
    _controller.add(SectionContainer.loading());
//    schemas = [];
  }

  Stream<SectionContainer<Shop>> onShopProfileUpdated() {
    return _controller.stream;
  }

  void loadShop() async {
    try {
      var initialized = await _api.userInit();
      if(initialized.error) {
        initialized = await _api.initUser();
        if(initialized.error) {
          // todo: some bs error message to retry
          print(initialized.message);
          _controller.add(SectionContainer.reject());
          return;
        }
        print("--- user initialized ---");
      }

      var response = await _api.getProfile();
      if (!response.error) {
        profile = response.data;
        _controller.add(SectionContainer.accept(data: profile));
        return;
      }
    } catch (e) {
      print(e);
    }
    _controller.add(SectionContainer.reject());
  }

  void createShop(Map<String, dynamic> form) async {
    _controller.add(SectionContainer.loading());
    try {
      var response = await _api.createShop(form);
      print("--- create shop response ---");
      print(response);
      if (response.error) {
        print(response.message);
      } else {
        profile = response.data;
        _controller.add(SectionContainer.accept(data: profile));
        return;
      }
    } catch (e) {
      print(e);
    }
    _controller.add(SectionContainer.reject());
  }
}
