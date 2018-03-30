using System.Collections.Generic;
using AppRopio.Base.Core.Models.Analytics;

namespace AppRopio.Analytics.MobileCenter.Core.Services
{
    public interface IMobileCenter
    {
        void TrackApp(AppState state, Dictionary<string, string> data);

        void TrackScreen(string screenName, ScreenState screenState, Dictionary<string, string> data);

        void TrackEvent(string category, string action, string label, Dictionary<string, string> data);

        void TrackException(string message, string stackTrace, Dictionary<string, string> data);
    }
}
