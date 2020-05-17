import 'package:moist_store/services/moist_client.dart';
import 'package:openid_client/openid_client_io.dart';
import 'package:url_launcher/url_launcher.dart';


class AuthenticationProvider {
  UserInfo _userInfo;
  TokenResponse _tokenResponse;

  authenticate() async {
    var uri = new Uri(host: "192.168.1.100", port: 8004, scheme: "http");
    var clientId = "flutter_shop";
    var issuer = await Issuer.discover(uri);
    var client = new Client(issuer, clientId);

    urlLauncher(String url) async {
      if (await canLaunch(url)) {
        await launch(url, forceWebView: true);
      } else {
        throw 'Could not launch $url';
      }
    }

    var authenticator = new Authenticator(client,
        scopes: ['user-api'], port: 4000, urlLancher: urlLauncher);

    var credentials = await authenticator.authorize();
    _tokenResponse = await credentials.getTokenResponse();
    _userInfo = await credentials.getUserInfo();

    closeWebView();

    return true;
  }

  String getAccessToken() {
    return _tokenResponse.accessToken;
  }
}
