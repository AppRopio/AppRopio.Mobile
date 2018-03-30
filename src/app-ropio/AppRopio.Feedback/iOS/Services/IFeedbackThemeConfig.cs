using System;
using AppRopio.Feedback.iOS.Models;

namespace AppRopio.Feedback.iOS.Services
{
	public interface IFeedbackThemeConfigService
	{
		FeedbackThemeConfig ThemeConfig { get; }
	}
}