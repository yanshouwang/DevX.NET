using DevX.Core.Win32;

namespace DevX.Core
{
    public class Service
    {
        public string Name { get; }
        public string DisplayName { get; }
        public int ProcessId { get; }
        public ServiceState State { get; }

        internal Service(string name, string displayName, SERVICE_STATUS status)
        {
            Name = name;
            DisplayName = displayName;
            State = (ServiceState)status.dwCurrentState;
        }

        internal Service(ENUM_SERVICE_STATUS_PROCESS status)
        {
            Name = status.lpServiceName;
            DisplayName = status.lpDisplayName;
            ProcessId = status.serviceStatusProcess.dwProcessId;
            State = (ServiceState)status.serviceStatusProcess.dwCurrentState;
        }

        internal Service(ENUM_SERVICE_STATUS status)
        {
            Name = status.lpServiceName;
            DisplayName = status.lpDisplayName;
            //ProcessId = status.serviceStatus.dwProcessId;
            State = (ServiceState)status.serviceStatus.dwCurrentState;
        }

        public override string ToString()
        {
            return $"{Name}: {DisplayName}";
        }

        public override bool Equals(object obj)
        {
            return obj is Service service && service.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
