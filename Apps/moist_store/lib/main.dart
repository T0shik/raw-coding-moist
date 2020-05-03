import 'package:flutter/material.dart';
import 'package:moist_store/pages/login.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:provider/provider.dart';

import 'pages/dashboard.dart';

void main() => runApp(MyApp());

class AuthenticationProvider with ChangeNotifier {
  Credential _credentials;
  UserInfo _userInfo;
  TokenResponse _tokenResponse;

  Future<void> setCredentials(Credential c) async {
    _credentials = c;
    _userInfo = await _credentials.getUserInfo();
    _tokenResponse = await _credentials.getTokenResponse();
  }

  String getAccessToken() {
    return _tokenResponse.accessToken;
  }
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => AuthenticationProvider()),
      ],
      child: Consumer<AuthenticationProvider>(
        builder: (context, auth, _) {
          return MaterialApp(
            onGenerateRoute: Router.generateRoute,
            initialRoute: '/',
          );
        },
      ),
    );
  }
}

class Router {
  static Route<dynamic> generateRoute(RouteSettings settings) {
    switch (settings.name) {
      case '/':
        return MaterialPageRoute(builder: (_) => LoginPage());
      case '/dashboard':
        return MaterialPageRoute(builder: (_) => Dashboard());
      default:
        return MaterialPageRoute(
            builder: (_) => Scaffold(
              body: Center(
                  child: Text('No route defined for ${settings.name}')),
            ));
    }
  }
}
