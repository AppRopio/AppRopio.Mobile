using System.Collections.Generic;
using AppRopio.Base.Core.Models.Analytics;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class ScreenAnalyticsMessage : BaseAnalyticsMessage
    {
        public ScreenState ScreenState { get; private set; }
        
        public string ScreenName { get; private set; }

        public ScreenAnalyticsMessage(object sender, string screenName, ScreenState screenState, Dictionary<string, string> data)
            : base (sender, data)
        {
            ScreenName = screenName;
            ScreenState = screenState;
        }
    }
}
