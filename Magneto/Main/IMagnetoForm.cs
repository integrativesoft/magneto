/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using System;
using System.ComponentModel;
using System.Drawing;

namespace Magneto.Main
{
    public enum WindowState
    {
        Normal,
        Maximized,
        Minimized
    }

    public interface IMagnetoForm
    {
        void SetIcon(Icon icon);
        Point Location { get; set; }
        Point Size { get; set; }
        bool Maximizable { get; set; }
        bool Minimizable { get; set; }
        double Opacity { get; set; }
        bool Resizable { get; set; }
        bool ShowInTaskbar { get; set; }
        string Title { get; set; }
        bool Topmost { get; set; }
        WindowState WindowState { get; set; }
        bool CenterOnShow { get; set; }

        Eto.Forms.Form EtoForm { get; }

        event EventHandler<EventArgs> Closed;
        event EventHandler<CancelEventArgs> Closing;
        event EventHandler<EventArgs> LocationChanged;
        event EventHandler<EventArgs> WindowStateChanged;
        event EventHandler<EventArgs> LogicalPixelSizeChanged;

        void RunApplication();
    }
}
