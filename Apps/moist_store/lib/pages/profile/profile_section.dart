import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/internals/section_container.dart';
import 'package:moist_store/internals/section_state.dart';
import 'package:moist_store/models/Shop.dart';
import 'package:moist_store/pages/profile/services/shop_provider.dart';
import 'package:provider/provider.dart';
import 'dashboard/dashboard.dart';
import 'setup_profile/setup_profile.dart';


class ProfileSectionWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    var shopProfile = context.select((ShopProfileProvider shopProfile) => shopProfile);
    shopProfile.loadShop();

    return StreamBuilder<SectionContainer<Shop>>(
      stream: shopProfile.onShopProfileUpdated(),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.active) {
          final section_state = snapshot.data;
          print("profile section state ${section_state.state} || data: ${section_state.data}");
          if (section_state.state == SectionState.Rejected) {
            return SetupProfileWidget();
          } else if (section_state.state == SectionState.Accepted) {
            return Provider.value(
              value: section_state.data,
              child: DashboardWidget(),
            );
          }
        }
        return Scaffold(
          body: Center(
            child: CircularProgressIndicator(),
          ),
        );
      },
    );
  }
}
