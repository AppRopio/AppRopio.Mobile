using System.Collections.Generic;
using AppRopio.Analytics.MobileCenter.Core.Services;
using AppRopio.Base.Core.Models.Analytics;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace AppRopio.Analytics.MobileCenter.iOS.Services
{
    public class MobileCenter : IMobileCenter
    {
        #region Constructor

        public MobileCenter()
        {
            Mvx.CallbackWhenRegistered<IMCConfigService>(service =>
            {
                Mvx.CallbackWhenRegistered<IMvxMainThreadDispatcher>(s => 
                {
                    s.RequestMainThreadAction(() => 
                    {
                        Microsoft.AppCenter.AppCenter.Start(service.AppCenterKey_IOS,
                                                            typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Microsoft.AppCenter.Crashes.Crashes));

                        Microsoft.AppCenter.Crashes.Crashes.GetErrorAttachments = (Microsoft.AppCenter.Crashes.ErrorReport report) =>
                        {
                            var bytes = Mvx.Resolve<Base.Core.Services.Log.ILogService>().CachedLog();
                            return new Microsoft.AppCenter.Crashes.ErrorAttachmentLog[]
                            {
                                Microsoft.AppCenter.Crashes.ErrorAttachmentLog.AttachmentWithBinary(bytes, "log.txt", "text")
                            };
                        };
                    });
                });
            });
        }

        #endregion

        #region Private

        private string GetAppEvent(AppState state)
        {
            switch (state)
            {
                case AppState.Started:
                    return "app_started";
                case AppState.Finished:
                    return "app_finished";
                case AppState.Background:
                    return "app_enter_background";
                case AppState.Foreground:
                    return "app_enter_foreground";
                default:
                    return string.Empty;
            }
        }

        private string GetScreenEvent(ScreenState state)
        {
            switch (state)
            {
                case ScreenState.Opened:
                    return "screen_opened";
                case ScreenState.Closed:
                    return "screen_closed";
                case ScreenState.Appeared:
                    return "screen_appeared";
                case ScreenState.Disappeared:
                    return "screen_disappeared";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region IMobileCenter implementation

        public void TrackApp(AppState state, Dictionary<string, string> data)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(GetAppEvent(state), new Dictionary<string, string>(data)
            {
                { "Category", "app" }
            });
        }

        public void TrackScreen(string screenName, ScreenState screenState, Dictionary<string, string> data)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(GetScreenEvent(screenState), new Dictionary<string, string>(data)
            {
                { "Category", "screen" },
                { "Label", screenName }
            });
        }

        public void TrackEvent(string category, string action, string label, Dictionary<string, string> data)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(action, new Dictionary<string, string>(data)
            {
                { "Category", category },
                { "Label", label }
            });
        }

        public void TrackException(string message, string stackTrace, Dictionary<string, string> data)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent("exception_handled", new Dictionary<string, string>(data)
            {
                { "Message", message },
                { "StackTrace", stackTrace }
            });
        }

        #endregion
    }
}
