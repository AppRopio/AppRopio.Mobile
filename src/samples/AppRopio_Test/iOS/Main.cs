using System;
using System.Diagnostics;
using UIKit;

namespace AppRopio.Test.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {

#if DEBUG
            AppRopio.Base.Core.ServicesDebug.StartupTimerService.Instance.StartTimer();
#endif
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Write(ex.BuildAllMessagesAndStackTrace());
                throw;
#endif
            }
        }
    }
}