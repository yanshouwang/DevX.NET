﻿namespace DevX.WindowsService.WinAPI
{
    internal enum SERVICE_TYPE
    {
        SERVICE_KERNEL_DRIVER = 0x00000001,
        SERVICE_FILE_SYSTEM_DRIVER = 0x00000002,
        SERVICE_ADAPTER = 0x00000004,
        SERVICE_RECOGNIZER_DRIVER = 0x00000008,
        SERVICE_WIN32_OWN_PROCESS = 0x00000010,
        SERVICE_WIN32_SHARE_PROCESS = 0x00000020,
        SERVICE_USER_OWN_PROCESS = 0x00000050,
        SERVICE_USER_SHARE_PROCESS = 0x00000060,
        SERVICE_INTERACTIVE_PROCESS = 0x00000100,
    }
}
