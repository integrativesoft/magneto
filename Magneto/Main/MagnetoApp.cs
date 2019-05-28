/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto;
using Eto.Forms;
using System;
using System.Runtime.InteropServices;

namespace Magneto.Main
{
    public static class MagnetoApp
    {
        public static IMagnetoForm CreateForm(Uri url)
        {
            BrowserFixer.FixSettingsIfNeeded();
            var platform = GetPlatform();
            var app = new Application(platform);
            return new MagnetoForm(app, url);
        }

        private static Platform GetPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Eto.Wpf.Platform();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new Eto.Mac.Platform();
            }
            else
            {
                return new Eto.GtkSharp.Platform();
            }
        }
    }
}
