/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Forms;
using System;
using System.Drawing;

namespace Integrative.Magneto
{
    sealed class MagnetoForm : Form, IMagnetoForm
    {
        readonly Application _app;

        public MagnetoForm(Application app, Uri url)
        {
            MinimumSize = new Eto.Drawing.Size(800, 600);
            Size = MinimumSize;
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

        public bool CenterOnShow { get; set; } = true;

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

        Point IMagnetoForm.MinimumSize
        {
            get => new Point(MinimumSize.Height, MinimumSize.Width);
            set => MinimumSize = new Eto.Drawing.Size(value.X, value.Y);
        }

        public void RunApplication()
        {
            _app.Run(this);
        }

        public void SetIcon(Icon icon)
        {
            Icon = CommonTools.IconToEto(icon);
        }

        public bool ConfirmClose(ConfirmCloseOptions options)
        {
            return AreYouSureForm.Run(this, options);
        }
    }
}
