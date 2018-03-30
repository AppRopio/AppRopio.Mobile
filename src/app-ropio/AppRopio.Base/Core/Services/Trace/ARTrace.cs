using System;
using System.Text;
using AppRopio.Base.Core.Services.Log;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Core.Services.Trace
{
    public class ARTrace : IMvxTrace
    {
        #region Services

        #endregion

        #region IMvxTrace implementation

        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            var traceMsg = tag + ":" + level + ":" + message();
#if DEBUG
            System.Diagnostics.Debug.WriteLine(traceMsg);
#endif
            Mvx.Resolve<ILogService>()?.Write(Encoding.UTF8.GetBytes(traceMsg));
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            var traceMsg = tag + ":" + level + ":" + message;
#if DEBUG
            System.Diagnostics.Debug.WriteLine(traceMsg);
#endif
            Mvx.Resolve<ILogService>()?.Write(Encoding.UTF8.GetBytes(traceMsg));
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                var traceMsg = string.Format("\n" + tag + ":" + level + ":" + message, args);
#if DEBUG
                System.Diagnostics.Debug.WriteLine(traceMsg, args);
#endif
                Mvx.Resolve<ILogService>()?.Write(Encoding.UTF8.GetBytes(traceMsg));
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, $"Exception during trace of {level} {message}");
            }
        }

        #endregion
    }
}
