using System;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Feedback.Core;
using AppRopio.Feedback.iOS.Models;

namespace AppRopio.Feedback.iOS.Services.Implementation
{
    public class FeedbackThemeConfigService : BaseThemeConfigService<FeedbackThemeConfig>, IFeedbackThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return FeedbackConstants.CONFIG_NAME;
            }
        }
    }
}