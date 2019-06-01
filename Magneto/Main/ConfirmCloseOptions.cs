/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

namespace Integrative.Magneto
{
    public class ConfirmCloseOptions
    {
        public string Title { get; set; } = "Close?";
        public string Message = "Close and exit?";
        public string CancelText = "Stay";
        public string ConfirmText = "Quit";
    }
}
