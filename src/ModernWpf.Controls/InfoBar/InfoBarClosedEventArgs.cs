﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWpf.Controls
{
    public class InfoBarClosedEventArgs : EventArgs
    {
        public InfoBarCloseReason Reason { get; internal set; }
    }
}
