using DevX.WindowsService.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DevX.WindowsService
{
    public class ServiceManager : IDisposable
    {
        private const string LOCAL_MACHINE_NAME = ".";

        private readonly IntPtr _managerPtr;

        public ServiceManager() : this(LOCAL_MACHINE_NAME)
        {

        }

        public ServiceManager(string machineName)
        {
            if (string.IsNullOrWhiteSpace(machineName) ||
                machineName.IndexOf('\\') != -1)
            {
                var message = $"{machineName} is invalid.";
                var paramName = nameof(machineName);
                throw new ArgumentException(message, paramName);
            }
            _managerPtr = ADVANCED_API.OpenSCManager(machineName, null, SC_MANAGER_ACCESS.SC_MANAGER_ALL_ACCESS);
            if (_managerPtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
        }

        public Service[] GetServices()
        {
            var serviceType =
                SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS |
                SERVICE_TYPE.SERVICE_WIN32_SHARE_PROCESS;
            var resumeHandle = 0;
            var enumerated = ADVANCED_API.EnumServicesStatusEx(
                _managerPtr,
                INFO_LEVEL.PROCESS_INFO,
                serviceType,
                SERVICE_ACTIVE_STATE.SERVICE_STATE_ALL,
                IntPtr.Zero, 0,
                out var bytesNeeded,
                out _,
                ref resumeHandle,
                null);
            if (enumerated)
            {
                return new Service[0];
            }
            else
            {
                var error = Marshal.GetLastWin32Error();
                if (error != ERROR.ERROR_MORE_DATA)
                {
                    throw new Win32Exception(error);
                }
            }
            var memoryPtr = Marshal.AllocHGlobal(bytesNeeded);
            try
            {
                enumerated = ADVANCED_API.EnumServicesStatusEx(
                    _managerPtr,
                    INFO_LEVEL.PROCESS_INFO,
                    serviceType,
                    SERVICE_ACTIVE_STATE.SERVICE_STATE_ALL,
                    memoryPtr, bytesNeeded,
                    out bytesNeeded,
                    out var servicesReturned,
                    ref resumeHandle,
                    null);
                if (!enumerated)
                {
                    var error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
                var services = new Service[servicesReturned];
                var statusSize = Marshal.SizeOf<ENUM_SERVICE_STATUS_PROCESS>();
                for (int i = 0; i < servicesReturned; i++)
                {
                    var statusPtr = memoryPtr + i * statusSize;
                    var status = Marshal.PtrToStructure<ENUM_SERVICE_STATUS_PROCESS>(statusPtr);
                    services[i] = new Service(status);
                }
                return services;
            }
            finally
            {
                Marshal.FreeHGlobal(memoryPtr);
            }
        }

        public Service[] GetDependentServices(Service service)
        {
            var servicePtr = ADVANCED_API.OpenService(
                _managerPtr, service.Name, SERVICE_ACCESS.SERVICE_ENUMERATE_DEPENDENTS);
            if (servicePtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            var enumerated = ADVANCED_API.EnumDependentServices(
                servicePtr,
                SERVICE_ACTIVE_STATE.SERVICE_STATE_ALL,
                IntPtr.Zero, 0,
                out var bytesNeeded,
                out _);
            if (enumerated)
            {
                return new Service[0];
            }
            else
            {
                var error = Marshal.GetLastWin32Error();
                if (error != ERROR.ERROR_MORE_DATA)
                {
                    throw new Win32Exception(error);
                }
            }
            var memoryPtr = Marshal.AllocHGlobal(bytesNeeded);
            try
            {
                enumerated = ADVANCED_API.EnumDependentServices(
                    servicePtr,
                    SERVICE_ACTIVE_STATE.SERVICE_STATE_ALL,
                    memoryPtr, bytesNeeded,
                    out bytesNeeded,
                    out var servicesReturned);
                if (!enumerated)
                {
                    var error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
                var services = new Service[servicesReturned];
                var statusSize = Marshal.SizeOf<ENUM_SERVICE_STATUS>();
                for (int i = 0; i < servicesReturned; i++)
                {
                    var statusPtr = memoryPtr + i * statusSize;
                    var status = Marshal.PtrToStructure<ENUM_SERVICE_STATUS>(statusPtr);
                    services[i] = new Service(status);
                }
                return services;
            }
            finally
            {
                Marshal.FreeHGlobal(memoryPtr);
            }
        }

        public Service Create(string name, string displayName, bool automatic, string binaryPath)
        {
            var startType = automatic
                ? SERVICE_START_TYPE.SERVICE_AUTO_START
                : SERVICE_START_TYPE.SERVICE_DEMAND_START;
            var servicePtr = ADVANCED_API.CreateService(
                _managerPtr, name, displayName,
                SERVICE_ACCESS.SERVICE_ALL_ACCESS,
                SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS, startType,
                SERVICE_ERROR.SERVICE_ERROR_NORMAL,
                binaryPath, null, IntPtr.Zero, null, null, null);
            if (servicePtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            var quried = ADVANCED_API.QueryServiceStatus(servicePtr, out var status);
            var closed = ADVANCED_API.CloseServiceHandle(servicePtr);
            if (!quried || !closed)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            return new Service(name, displayName, status);
        }

        public void Delete(Service service)
        {
            var servicePtr = ADVANCED_API.OpenService(_managerPtr, service.Name, SERVICE_ACCESS.DELETE);
            if (servicePtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            var deleted = ADVANCED_API.DeleteService(servicePtr);
            var closed = ADVANCED_API.CloseServiceHandle(servicePtr);
            if (!deleted || !closed)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
        }

        public void Start(Service service)
        {
            Start(service, null);
        }

        public void Start(Service service, string[] args)
        {
            var servicePtr = ADVANCED_API.OpenService(_managerPtr, service.Name, SERVICE_ACCESS.SERVICE_START);
            if (servicePtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            var started = ADVANCED_API.StartService(servicePtr, args.Length, args);
            var closed = ADVANCED_API.CloseServiceHandle(servicePtr);
            if (!started || !closed)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
        }

        public void Stop(Service service)
        {
            var servicePtr = ADVANCED_API.OpenService(_managerPtr, service.Name, SERVICE_ACCESS.SERVICE_STOP);
            if (servicePtr == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            //var size = Marshal.SizeOf<SERVICE_STATUS>();
            //var statusPtr = Marshal.AllocHGlobal(size);
            //var stopped = Advapi32.ControlService(servicePtr, SERVICE_CONTROL.SERVICE_CONTROL_STOP, statusPtr);
            //var status = Marshal.PtrToStructure<SERVICE_STATUS>(statusPtr);
            //Marshal.FreeHGlobal(statusPtr);
            var stopped = ADVANCED_API.ControlService(servicePtr, SERVICE_CONTROL.SERVICE_CONTROL_STOP, out _);
            var closed = ADVANCED_API.CloseServiceHandle(servicePtr);
            if (!stopped || !closed)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
        }

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    ADVANCED_API.CloseServiceHandle(_managerPtr);
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                _disposed = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~WindowsServiceController()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
