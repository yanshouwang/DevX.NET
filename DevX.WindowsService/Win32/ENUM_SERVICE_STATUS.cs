using System.Runtime.InteropServices;

namespace DevX.WindowsService.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class ENUM_SERVICE_STATUS
    {
        public string lpServiceName;
        public string lpDisplayName;
        public SERVICE_STATUS serviceStatus;
    }
}
