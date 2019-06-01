/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Forms;
using System;

namespace Magneto.Main
{
    public static class MagnetoApp
    {
        public static IMagnetoForm CreateForm(Uri url)
        {
            return CreateForm(url, new Application());
        }

        public static IMagnetoForm CreateForm(Uri url, Application app)
        {
            BrowserFixer.FixSettingsIfNeeded();
            return new MagnetoForm(app, url);
        }
    }
}
