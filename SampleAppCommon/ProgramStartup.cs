/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Integrative.Lara;
using Integrative.Magneto;
using System;

namespace SampleApp
{
    public static class ProgramStartup
    {
        public static void Main()
        {
            // create home page
            LaraUI.Publish("/", () => new KitchenSinkForm());

            // start web server
            var host = LaraUI.StartServer().Result;

            // get address (default setting is to assign a dynamic port)
            string address = LaraUI.GetFirstURL(host);

            // run application
            using (var session = MagnetoApp.CreateForm(new Uri(address)))
            {
                ConfigureConfirmExit(session);
                session.RunApplication();
            }
        }

        private static void ConfigureConfirmExit(IMagnetoForm session)
        {
            session.Closing += (sender, args) =>
            {
                args.Cancel = !session.ConfirmClose(new ConfirmCloseOptions
                {
                    Title = "Close and exit",
                    CancelText = "Stay",
                    ConfirmText = "Quit",
                    Message = "Are you sure?"
                });
            };
        }
    }
}