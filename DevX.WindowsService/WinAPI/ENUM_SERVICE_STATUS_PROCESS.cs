﻿using System.Runtime.InteropServices;

namespace DevX.WindowsService.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class ENUM_SERVICE_STATUS_PROCESS
    {
        public string lpServiceName;
        public string lpDisplayName;
        public SERVICE_STATUS_PROCESS serviceStatusProcess;
    }
}
