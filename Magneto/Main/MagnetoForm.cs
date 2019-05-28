/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Forms;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Magneto.Main
{
    sealed class MagnetoForm : Form, IMagnetoForm
    {
        readonly Application _app;

        public MagnetoForm(Application app, Uri url)
        {
            _app = app;
            var browser = new WebView
            {
                Url = url
            };
            Content = browser;
            Title = "Magneto";
            Load += MagnetoForm_Shown;
            Icon = CommonTools.LoadIconResource("Magneto.Main.favicon.ico");
            new BrowserFixer(browser);
        }

        private void MagnetoForm_Shown(object sender, EventArgs e)
        {
            if (CenterOnShow)
            {
                Location = new Eto.Drawing.Point
                {
                    X = (int)Math.Round((Screen.WorkingArea.Width - Width) / 2),
                    Y = (int)Math.Round((Screen.WorkingArea.Height - Height) / 2)
                };
            }
        }

        public bool CenterOnShow { get; set; }

        Point IMagnetoForm.Location
        {
            get => new Point(Location.X, Location.Y);
            set
            {
                Location = new Eto.Drawing.Point(value.X, value.Y);
            }
        }

        WindowState IMagnetoForm.WindowState
        {
            get => (WindowState)(int)WindowState;
            set => WindowState = (Eto.Forms.WindowState)(int)value;
        }

        Point IMagnetoForm.Size
        {
            get => new Point(Width, Height);
            set => Size = new Eto.Drawing.Size(value.X, value.Y);
        }

        public Form EtoForm => this;

        public void RunApplication()
        {
            _app.Run(this);
        }

        public void SetIcon(Icon icon)
        {
            Icon = CommonTools.IconToEto(icon);
        }
    }
}
