using System.Collections.Generic;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class BaseAnalyticsMessage : MvxMessage
    {
        public Dictionary<string, string> Data { get; }

        public BaseAnalyticsMessage(object sender, Dictionary<string, string> data = null)
            : base (sender)
        {
            Data = data;
        }
    }
}
