/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Eto.Forms;

namespace Integrative.Magneto
{
    sealed class AreYouSureForm : Dialog
    {
        public static bool Run(Form parent, ConfirmCloseOptions options)
        {
            using (var dialog = new AreYouSureForm(options))
            {
                dialog.ShowInTaskbar = false;
                dialog.ShowModal(parent);
                return dialog.Confirmed;
            }
        }

        private bool Confirmed { get; set; }

        private AreYouSureForm(ConfirmCloseOptions options)
        {
            ShowInTaskbar = false;
            MinimumSize = new Eto.Drawing.Size(250, 100);
            var layout = new DynamicLayout();
            var cancel = new Button();
            var close = new Button();
            DefaultButton = close;
            AbortButton = cancel;
            Maximizable = false;
            Minimizable = false;
            Resizable = false;
            Title = options.Title;
            layout.Padding = new Eto.Drawing.Padding(10, 0);
            cancel.Text = options.CancelText;
            close.Text = options.ConfirmText;
            layout.BeginVertical(padding: new Eto.Drawing.Padding(0, 10), yscale: true);
            layout.Add(new Label { Text = options.Message });
            layout.EndVertical();
            layout.BeginVertical(padding: new Eto.Drawing.Padding(0, 10), spacing: new Eto.Drawing.Size(5, 0));
            layout.BeginHorizontal();
            layout.Add(null);
            layout.Add(cancel);
            layout.Add(close);
            layout.EndHorizontal();
            layout.EndVertical();
            Content = layout;
            cancel.Click += (sender, args) => Close();
            close.Click += (sender, args) =>
            {
                Confirmed = true;
                Close();
            };
        }
    }
}
