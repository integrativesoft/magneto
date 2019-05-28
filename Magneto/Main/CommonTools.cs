/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Drawing;
using System.IO;
using System.Reflection;

namespace Magneto.Main
{
    static class CommonTools
    {
        public static Icon IconToEto(System.Drawing.Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return new Icon(ms);
            }
        }

        public static Icon LoadIconResource(string name)
        {
            var assembly = Assembly.GetAssembly(typeof(CommonTools));
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                return new Icon(stream);
            }
        }
    }
}
