using System.Collections.Generic;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class ExceptionAnalyticsMessage : BaseAnalyticsMessage
    {
        public string Message { get; }

        public string StackTrace { get; }

        public bool IsFatal { get; }

        public ExceptionAnalyticsMessage(object sender, string message, string stackTrace, bool isFatal, Dictionary<string, string> data)
            : base(sender, data)
        {
            StackTrace = stackTrace;
            IsFatal = isFatal;
            Message = message;
        }
    }
}
