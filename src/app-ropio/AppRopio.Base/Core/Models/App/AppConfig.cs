using System;
namespace AppRopio.Base.Core.Models.App
{
    public class AppConfig
    {
        public int RequestTimeoutInSeconds { get; set; }

        public string ApiKey { get; set; }

        public string AppID { get; set; }

        public string AppLabel { get; set; }

        public string CompanyID { get; set; }

        public string DefaultRegionID { get; set; }

        public string ErrorWhenConnectionFailed { get; set; }

        public string ErrorWhenRequestCancelled { get; set; }

        public string Host { get; set; }

        public bool PreciseCurrency { get; set; }
    }
}

