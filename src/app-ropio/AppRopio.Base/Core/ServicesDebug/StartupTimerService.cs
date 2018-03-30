using System;
using System.Diagnostics;

namespace AppRopio.Base.Core.ServicesDebug
{
    public class StartupTimerService
    {
        private Stopwatch _stopwatch;

        public static StartupTimerService Instance { get; }

        private StartupTimerService()
        {
            _stopwatch = new Stopwatch();
        }

        static StartupTimerService()
        {
            Instance = new StartupTimerService();
        }

        public void StartTimer()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public long EllapsedMilliseconds()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        public void StopTimer()
        {
            _stopwatch.Stop();
        }
    }
}
