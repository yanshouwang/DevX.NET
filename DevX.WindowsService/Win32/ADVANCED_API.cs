using System;
using System.Runtime.InteropServices;

namespace DevX.WindowsService.Win32
{
    internal class ADVANCED_API
    {
        private const string DLL_NAME = "advapi32.dll";

        [DllImport(DLL_NAME, EntryPoint = "OpenSCManagerW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, SC_MANAGER_ACCESS dwDesiredAccess);

        [DllImport(DLL_NAME, EntryPoint = "EnumServicesStatusW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool EnumServicesStatus(
            IntPtr hSCManager,
            SERVICE_TYPE dwServiceType,
            SERVICE_ACTIVE_STATE dwServiceState,
            IntPtr lpServices,
            int cbBufSize,
            out int pcbBytesNeeded,
            out int lpServicesReturned,
            ref int lpResumeHandle);

        [DllImport(DLL_NAME, EntryPoint = "EnumServicesStatusExW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool EnumServicesStatusEx(
            IntPtr hSCManager,
            INFO_LEVEL infoLevel,
            SERVICE_TYPE dwServiceType,
            SERVICE_ACTIVE_STATE dwServiceState,
            IntPtr lpServices,
            int cbBufSize,
            out int pcbBytesNeeded,
            out int lpServicesReturned,
            ref int lpResumeHandle,
            string pszGroupName);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateService(
            IntPtr hSCManager,
            string lpServiceName,
            string lpDisplayName,
            SERVICE_ACCESS dwDesiredAccess,
            SERVICE_TYPE dwServiceType,
            SERVICE_START_TYPE dwStartType,
            SERVICE_ERROR dwErrorControl,
            string lpBinaryPathName,
            string lpLoadOrderGroup,
            IntPtr lpdwTagId,
            string lpDependencies,
            string lpServiceStartName,
            string lpPassword);

        [DllImport(DLL_NAME, EntryPoint = "OpenServiceW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, SERVICE_ACCESS dwDesiredAccess);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CloseServiceHandle(IntPtr hSCObject);

        [DllImport(DLL_NAME, EntryPoint = "EnumDependentServicesW", ExactSpelling = true, SetLastError = true)]
        internal static extern bool EnumDependentServices(
            IntPtr hService,
            SERVICE_ACTIVE_STATE dwServiceState,
            IntPtr lpServices,
            int cbBufSize,
            out int pcbBytesNeeded,
            out int lpServicesReturned);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceStatus(IntPtr hService, out SERVICE_STATUS lpServiceStatus);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceStatusEx(IntPtr hService, INFO_LEVEL infoLevel, IntPtr lpBuffer, int cbBufSize, out int pcbBytesNeeded);

        [DllImport(DLL_NAME, EntryPoint = "StartServiceW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool StartService(IntPtr hService, int dwNumServiceArgs, string[] lpServiceArgVectors);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool ControlService(IntPtr hService, SERVICE_CONTROL dwControl, out SERVICE_STATUS lpServiceStatus);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteService(IntPtr hService);
    }
}
