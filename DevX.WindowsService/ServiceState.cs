using DevX.WindowsService.WinAPI;

namespace DevX.WindowsService
{
    public enum ServiceState
    {
        Stopped = SERVICE_STATE.SERVICE_STOPPED,
        StartPending = SERVICE_STATE.SERVICE_START_PENDING,
        StopPending = SERVICE_STATE.SERVICE_STOP_PENDING,
        Running = SERVICE_STATE.SERVICE_RUNNING,
        ContinuePending = SERVICE_STATE.SERVICE_CONTINUE_PENDING,
        PausePending = SERVICE_STATE.SERVICE_PAUSE_PENDING,
        Paused = SERVICE_STATE.SERVICE_PAUSED,
    }
}
