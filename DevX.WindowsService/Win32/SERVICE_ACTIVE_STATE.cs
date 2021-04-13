using System;

namespace DevX.WindowsService.Win32
{
    [Flags]
    internal enum SERVICE_ACTIVE_STATE
    {
        SERVICE_ACTIVE = 0x00000001,
        SERVICE_INACTIVE = 0x00000002,
        SERVICE_STATE_ALL = 0x00000003,
    }
}
