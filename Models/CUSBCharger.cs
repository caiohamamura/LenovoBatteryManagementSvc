﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IdeapadToolkit.Models
{
    [NativeCppClass]
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct CUSBCharger
    {
        private long value;
    }
}
