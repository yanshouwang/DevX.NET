using System.Runtime.InteropServices;

namespace DevX.WindowsService.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SERVICE_STATUS
    {
        public SERVICE_TYPE dwServiceType;
        public SERVICE_STATE dwCurrentState;
        public SERVICE_ACCEPT dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    }
}
