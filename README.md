![.NET Core Desktop](https://github.com/glennawatson/Xamarin.Forms.Auth/workflows/.NET%20Core%20Desktop/badge.svg)
## Xamarin.Forms.Auth

Xamarin.Forms.Auth is a OAuth2 library for the Xamarin.Forms platforms. It will allow users to authenticate against OAuth2 servers.

It will cache token response for a particular client inside Xamarin.Essentials libary secure blobs.

Originally based on [Microsoft Authentication Library](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet).

## Install the NuGet package

> Install-package Xamarin.Forms.Auth

## Inside your Net Standard project

``` csharp
public class OAuth2LoginService
{
    // assumes you have made a class called AuthenticationConfig
    private static readonly IPublicClientApplication AuthenticationClient = PublicClientApplicationBuilder
              .Create(AuthenticationConfig.ClientId)
              .WithAuthority(AuthenticationConfig.Authority)
              .WithRedirectUri(AuthenticationConfig.RedirectUrl)
              .WithExtraQueryParameters(AuthenticationConfig.AdditionalQueryHeaders)
              .Build();

    public OAuth2LoginService()
    {
        _currentUserName = _authenticationResults.Select(x => x.GetParsedIdToken().GetUniqueId()).ToProperty(this, nameof(CurrentUserName));
    }

    /// <inheritdoc />
    public async Task<string> GetLoginToken(CancellationToken token = default)
    {
        AuthenticationResult authResult;

        // let's see if we have the user details already available.
        try
        {
            authResult = await AuthenticationClient.AcquireTokenSilent(AuthenticationConfig.Scopes).ExecuteAsync(token).ConfigureAwait(false);
        }
        catch (AuthUiRequiredException)
        {
            try
            {
                authResult = await AuthenticationClient.AcquireTokenInteractive(AuthenticationConfig.Scopes)
                                             .WithParentActivityOrWindow(App.ParentWindow)
                                             .ExecuteAsync(token)
                                             .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Log().Warn(ex, "Could not log into the authentication system");
                return null;
            }
        }

        return authResult.AccessToken;
    }
}
```

## Xamarin Android specific

On Xamarin.Android, you need to set the parent activity so that the token gets back once the interaction has happened. Often we do this by setting the main activity as a static members of the App class.

```cs
var authResult = AcquireTokenInteractive(scopes)
 .WithParentActivityOrWindow(App.ParentWindow)
 .ExecuteAsync();
 ```

Inside your App.xaml.cs add a property for the Parent Window.

```cs
class App
{
    /// <summary>
    /// Gets or sets the parent window or activity.
    /// </summary>
    public static object ParentWindow { get; set; }
}
```

Then in your `MainActivity.cs` set the ParentWindow. Make sure that Xamarin.Essentials is initilized also.

```cs
public class MainActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        App.ParentWindow = this;
        // Other initialization stuff.
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
        LoadApplication(new App());
    }
}
```

You need to override the `OnActivityResult` method of the main Activity and call the `SetAuthenticationContinuationEventArgs` method of the `AuthenticationContinuationHelper` class. That line ensures that the control goes back to Xamarin.Forms.Auth once the interactive portion of the authentication flow ended.

```cs
protected override void OnActivityResult(int requestCode, 
                                         Result resultCode, Intent data)
{
 base.OnActivityResult(requestCode, resultCode, data);
 AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode,
                                                                         resultCode,
                                                                         data);
}
```

We use a Intent filter to indicate when functionality should return back to the main activity. You need to modify the `AndroidManifest.xml` with the new intent filter inside the `Application` tag. Make sure you add your redirect URL.

```xml
<!--Intent filter to capture System Browser calling back to our app after Sign In-->
<activity
    android:name="xamarin.forms.auth.BrowserTabActivity">
    <intent-filter>
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.DEFAULT" />
    <category android:name="android.intent.category.BROWSABLE" />
    <data android:scheme="https" android:host="<REDIRECT-URL>" />
    </intent-filter>
</activity>
```

## iOS Specific

First you need to override the `OpenUrl` method of the `FormsApplicationDelegate` derived class and call `AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs`.

```cs
public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
{
    AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
    return true;
}
```

To enable keychain access, your application must have a keychain access group. You can set your keychain access group by using the WithIosKeychainSecurityGroup() api when creating your application as shown below:

```cs
var builder = PublicClientApplicationBuilder
     .Create(ClientId)
     .WithIosKeychainSecurityGroup("com.myapp.rules")
     .Build();
```

The entitlements.plist should be updated to look like the following:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  <key>keychain-access-groups</key>
  <array>
    <string>$(AppIdentifierPrefix)com.myapp.rules</string>
  </array>
</dict>
</plist>
```
