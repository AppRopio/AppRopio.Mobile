using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Launcher;
using Foundation;
using SafariServices;
using UIKit;

namespace AppRopio.Base.iOS.Services.Launcher
{
    public class LauncherService : ILauncherService
	{
		public Task LaunchEmail(string email)
		{
			var url = new NSUrl("mailto:" + email);

            if (UIApplication.SharedApplication.CanOpenUrl(url))
    			UIApplication.SharedApplication.OpenUrl(url);

			return Task.CompletedTask;
		}

		public Task LaunchPhone(string phoneNumber)
		{
			var url = new NSUrl("tel:" + phoneNumber);

            if (UIApplication.SharedApplication.CanOpenUrl(url))
    			UIApplication.SharedApplication.OpenUrl(url);

			return Task.CompletedTask;
		}

		public Task LaunchUri(string uri)
		{
			var url = new NSUrl(uri);
           
            if (UIApplication.SharedApplication.CanOpenUrl(url))
			{
                var safariVC = new SFSafariViewController(url);
                var rootVC = UIApplication.SharedApplication.KeyWindow.RootViewController;

                if (rootVC != null)
                {
                    rootVC.PresentViewController(safariVC, true, null);
                }
            }

			return Task.CompletedTask;
		}

		public Task LaunchAddress(string address)
		{
			var url = new NSUrl("http://maps.apple.com/?q=" + Uri.EscapeDataString(address));

            if (UIApplication.SharedApplication.CanOpenUrl(url))
    			UIApplication.SharedApplication.OpenUrl(url);

			return Task.CompletedTask;
		}
	}
}