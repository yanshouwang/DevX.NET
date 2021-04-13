using System.Runtime.InteropServices;

namespace DevX.WindowsService.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class SERVICE_STATUS_PROCESS
    {
        public SERVICE_TYPE dwServiceType;
        public SERVICE_STATE dwCurrentState;
        public SERVICE_ACCEPT dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
        public int dwProcessId;
        public SERVICE_FLAGS dwServiceFlags;
    }
}
