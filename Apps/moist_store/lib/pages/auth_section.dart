import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:moist_store/internals/section_container.dart';
import 'package:moist_store/internals/section_state.dart';
import 'package:moist_store/models/User.dart';
import 'package:moist_store/pages/profile/services/api_provider.dart';
import 'package:moist_store/pages/profile/services/shop_provider.dart';
import 'package:moist_store/services/authentication_provider.dart';
import 'package:provider/provider.dart';
import 'login.dart';
import 'profile/profile_section.dart';

class AuthSectionWidget extends StatelessWidget {


  @override
  Widget build(BuildContext context) {
    print("building auth section!");
    var authService = context.select((AuthenticationProvider auth) => auth);
    authService.init();

    return StreamBuilder<SectionContainer<User>>(
      stream: authService.onUserAuthenticated(),
      initialData: SectionContainer.loading(),
      builder: (context, snapshot) {

        if (snapshot.connectionState == ConnectionState.active) {

          final section_state = snapshot.data;
          print("auth section state ${section_state.state} || data: ${section_state.data}");

          if (section_state.state == SectionState.Rejected) {
            return LoginPage();
          } else if (section_state.state == SectionState.Accepted) {
            return MultiProvider(
              providers: [
                Provider<ApiProvider>(create: (_) => ApiProvider(authService.getAccessToken())),
                ProxyProvider<ApiProvider, ShopProfileProvider>(
                  update: (_, api, __) => ShopProfileProvider(api),
                ),
              ],
              child: ProfileSectionWidget(),
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
