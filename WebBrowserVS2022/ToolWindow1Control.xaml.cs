using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WebBrowserVS2022
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Interaction logic for ToolWindow1Control.
    /// </summary>
    public partial class ToolWindow1Control : UserControl
    {
        private bool IsLoading { get; set; }
        private string UserDataFolder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        public ToolWindow1Control()
        {
            InitUserDataFolder();
            this.InitializeComponent();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            webView2.CoreWebView2InitializationCompleted += WebView2Core_Initialized;
            webView2.NavigationStarting += WebView2_NavigationStarting;
            webView2.NavigationCompleted += WebView2_NavigationCompleted;

            var env = await CoreWebView2Environment.CreateAsync(null, UserDataFolder);
            _ = webView2.EnsureCoreWebView2Async(env);
        }

        private void InitUserDataFolder()
        {
            var temp = System.IO.Path.GetTempPath();

            UserDataFolder = System.IO.Path.Combine(temp, "WebBrowserVS2022_UserData");
            System.IO.Directory.CreateDirectory(UserDataFolder);
        }

        private string GetHomePage()
        {
            var homepage = General.Instance.HomePage;
            System.Uri uri = null;
            if (System.Uri.TryCreate(homepage, System.UriKind.Absolute, out uri))
            {
                if (uri.Scheme == System.Uri.UriSchemeHttps ||
                    uri.Scheme == System.Uri.UriSchemeHttp ||
                    uri.Scheme == System.Uri.UriSchemeFile)
                {
                    return homepage;
                }
            }
            
            return "https://bing.com/";
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                webView2.GoBack();
            }
        }

        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                webView2.GoForward();
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                if (IsLoading)
                {
                    webView2.Stop();
                }
                else
                {
                    webView2.Reload();
                }
            }
        }

        private void ButtonHomePage_Click(object sender, RoutedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                webView2.CoreWebView2.Navigate(GetHomePage());
            }
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                if (addressBar.Text.StartsWith("https://") ||
                    addressBar.Text.StartsWith("http://") ||
                    addressBar.Text.StartsWith("file:///"))
                {
                    webView2.CoreWebView2.Navigate(addressBar.Text);
                }
            }
        }

        private void AddressBar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ButtonGo_Click(this, new RoutedEventArgs());
            }
        }

        private void WebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            buttonRefresh.Content = FindResource("iconStop");
            IsLoading = true;
        }

        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            buttonRefresh.Content = FindResource("iconRefresh");
            IsLoading = false;
        }

        private void WebView2Core_Initialized(object sender, System.EventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                webView2.CoreWebView2.NewWindowRequested += WebView2_NewWindowRequested;
                webView2.CoreWebView2.Navigate(GetHomePage());
            }
        }

        private void WebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (webView2 != null && webView2.CoreWebView2 != null)
            {
                // This prevents opening in new window.
                e.NewWindow = webView2.CoreWebView2;
            }
        }
    }
}