﻿/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using System;

namespace Magneto.Main
{
    public class ClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
