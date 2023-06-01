using Microsoft.Web.WebView2.Core;

namespace Health.Mobile.Platforms.Windows;

internal interface IPermissionRequestHandler
{
    void OnPermissionRequested(CoreWebView2 sender, CoreWebView2PermissionRequestedEventArgs args);
}
