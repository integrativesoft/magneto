/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Forms;
using System.Runtime.InteropServices;

namespace Magneto.Main
{
    sealed class BrowserFixer
    {
        readonly WebView _browser;

        public BrowserFixer(WebView view)
        {
            _browser = view;
            Fix();
        }

        private void Fix()
        {
            _browser.OpenNewWindow += Browser_OpenNewWindow;
        }

        private static void Browser_OpenNewWindow(object sender, WebViewNewWindowEventArgs e)
        {
            e.Cancel = true;
        }

        public static void FixSettingsIfNeeded()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IECompatibility.FixModeIfNeeded();
            }
        }
    }
}
