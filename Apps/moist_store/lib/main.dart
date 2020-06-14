import 'package:flutter/material.dart';
import 'package:moist_store/pages/auth_section.dart';
import 'package:provider/provider.dart';

import 'services/authentication_provider.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        Provider(create: (_) => AuthenticationProvider()),
      ],
      child: MaterialApp(
          theme: ThemeData(primarySwatch: Colors.indigo),
          home: AuthSectionWidget()
      ),
    );
  }
}
