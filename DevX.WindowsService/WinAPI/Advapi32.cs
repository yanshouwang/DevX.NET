using System;
using System.Runtime.InteropServices;

namespace DevX.WindowsService.WinAPI
{
    internal class Advapi32
    {
        private const string ADVAPI32 = "advapi32.dll";

        [DllImport(ADVAPI32, EntryPoint = "OpenSCManagerW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, SC_MANAGER_ACCESS dwDesiredAccess);

        [DllImport(ADVAPI32, EntryPoint = "EnumServicesStatusW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool EnumServicesStatus(
            IntPtr hSCManager,
            SERVICE_TYPE dwServiceType,
            SERVICE_ACTIVE_STATE dwServiceState,
            IntPtr lpServices,
            int cbBufSize,
            out int pcbBytesNeeded,
            out int lpServicesReturned,
            ref int lpResumeHandle);

        [DllImport(ADVAPI32, EntryPoint = "EnumServicesStatusExW", CharSet = CharSet.Unicode, SetLastError = true)]
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

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
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

        [DllImport(ADVAPI32, EntryPoint = "OpenServiceW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, SERVICE_ACCESS dwDesiredAccess);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CloseServiceHandle(IntPtr hSCObject);

        [DllImport(ADVAPI32, EntryPoint = "EnumDependentServicesW", ExactSpelling = true, SetLastError = true)]
        internal static extern bool EnumDependentServices(
            IntPtr hService,
            SERVICE_ACTIVE_STATE dwServiceState,
            IntPtr lpServices,
            int cbBufSize,
            out int pcbBytesNeeded,
            out int lpServicesReturned);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceStatus(IntPtr hService, out SERVICE_STATUS lpServiceStatus);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceStatusEx(IntPtr hService, INFO_LEVEL infoLevel, IntPtr lpBuffer, int cbBufSize, out int pcbBytesNeeded);

        [DllImport(ADVAPI32, EntryPoint = "StartServiceW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool StartService(IntPtr hService, int dwNumServiceArgs, string[] lpServiceArgVectors);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool ControlService(IntPtr hService, SERVICE_CONTROL dwControl, out SERVICE_STATUS lpServiceStatus);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteService(IntPtr hService);
    }
}
