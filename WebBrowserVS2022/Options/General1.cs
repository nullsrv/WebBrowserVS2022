using System.ComponentModel;
using Community.VisualStudio.Toolkit;

namespace WebBrowserVS2022
{
    internal partial class OptionsProvider
    {
        // Register the options with these attributes on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.General1Options), "WebBrowserVS2022", "General1", 0, 0, true)]
        // [ProvideProfile(typeof(OptionsProvider.General1Options), "WebBrowserVS2022", "General1", 0, 0, true)]
        public class General1Options : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        [Category("General")]
        [DisplayName("Homepage")]
        [Description("Website loaded on start or when pressing home button")]
        [DefaultValue("https://bing.com/")]
        public string HomePage { get; set; } = "https://bing.com/";
    }
}
