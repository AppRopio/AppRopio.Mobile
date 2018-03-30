using System.Collections.Generic;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class EventAnalyticsMessage : BaseAnalyticsMessage
    {
        public string Category { get; private set; }

        public string Action { get; private set; }

        public string Label { get; private set; }

        public object Model { get; private set; }

        public EventAnalyticsMessage(object sender, string category, string action, string label = null, object model = null, Dictionary<string, string> data = null)
            : base(sender, data)
        {
            Model = model;
            Label = label;
            Action = action;
            Category = category;
        }
    }
}
