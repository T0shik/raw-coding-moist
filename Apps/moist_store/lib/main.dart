import 'package:flutter/material.dart';
import 'package:moist_store/pages/login.dart';
import 'package:moist_store/pages/profile/create_shop_profile.dart';
import 'package:moist_store/services/authentication_provider.dart';
import 'package:moist_store/services/moist_client.dart';
import 'package:provider/provider.dart';

import 'pages/dashboard.dart';

void main() => runApp(MyApp());


class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    print("Hello World");

    return MultiProvider(
      providers: [
        Provider(create: (_) => AuthenticationProvider()),
        ProxyProvider<AuthenticationProvider, MoistClient>(
          update: (_, auth, __) => MoistClient(auth: auth),
        )
      ],
      child: Consumer<AuthenticationProvider>(
        builder: (context, auth, _) {
          return MaterialApp(
            initialRoute: '/login',
            routes: {
              '/login': (_) => LoginPage(),
              '/dashboard':(_) => Dashboard(),
              '/profile/create': (_) => CreateProfileForm(),
            },
          );
        },
      ),
    );
  }
}
