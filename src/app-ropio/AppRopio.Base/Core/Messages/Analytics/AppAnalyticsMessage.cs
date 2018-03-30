using System.Collections.Generic;
using AppRopio.Base.Core.Models.Analytics;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class AppAnalyticsMessage : BaseAnalyticsMessage
    {
        public AppState State { get; private set; }

        public AppAnalyticsMessage(object sender, AppState state, Dictionary<string, string> data)
            : base (sender, data)
        {
            State = state;
        }
    }
}
