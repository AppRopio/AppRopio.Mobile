using System;
using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.Models.OAuth;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.iOS.UIExtentions;
using Facebook.LoginKit;
using Foundation;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross;
using UIKit;
using VKontakte;
using VKontakte.Core;

namespace AppRopio.Base.Auth.iOS
{
    public class OAuthService : IOAuthService
	{
		private AppRopioVKSdkDelegate _appRopioVKSdkDelegate;
		private AppRopioVKSdkUiDelegate _appRopioVkUiDelegate;

		public OAuthService()
		{
			

			var dict = NSDictionary.FromFile("Info.plist");
			NSObject vkAppId;
			if (dict.TryGetValue(new NSString("VkAppID"), out vkAppId))
			{ 
				_appRopioVKSdkDelegate = new AppRopioVKSdkDelegate();
				_appRopioVkUiDelegate = new AppRopioVKSdkUiDelegate();
				var sdkInstance = VKSdk.Initialize(vkAppId.ToString());
				sdkInstance.RegisterDelegate(_appRopioVKSdkDelegate);
				sdkInstance.UiDelegate = _appRopioVkUiDelegate;
			}


		}


		private UIViewController GetLastPresentedController(UIViewController lastNotNullPresentedController)
		{
			return lastNotNullPresentedController.PresentedViewController != null ? GetLastPresentedController(lastNotNullPresentedController.PresentedViewController) : lastNotNullPresentedController;
		}

		public async Task<string> SignInTo(OAuthType socialType)
		{
			switch (socialType)
			{
				case OAuthType.Facebook:
					return await FacebookLogin();
				case OAuthType.VK:
					return await VKLogin();
			}

			return null;
		}


		#region Facebook

		private async Task<string> FacebookLogin()
		{
			var currentToken = Facebook.CoreKit.AccessToken.CurrentAccessToken;
			if (currentToken != null && currentToken.ExpirationDate.NSDateToDateTime() > DateTime.Now)
				return currentToken.TokenString;

			var login = new LoginManager();
			login.LoginBehavior = LoginBehavior.SystemAccount;

			var masterNavigationController = ((MvxIosViewPresenter)Mvx.Resolve<IMvxIosViewPresenter>()).MasterNavigationController;
			var targetVC = GetLastPresentedController(masterNavigationController);

			var loginResult = await login.LogInWithReadPermissionsAsync(
				new[] { @"public_profile", @"email" },
				targetVC
			);

			if (loginResult.IsCancelled)
				throw new OperationCanceledException();
			else
				return loginResult.Token.TokenString;
		}

		#endregion

		#region VK

		private Task<string> VKLogin()
		{
			if (_appRopioVKSdkDelegate == null)
				throw new Exception("VK not initialized, try to add VkAppID key to Info.plist");
			
			var tcs = new TaskCompletionSource<string>();
			_appRopioVKSdkDelegate.TCS = tcs;

			const string offlineScope = "offline";
			const string emailScope = "email";

			try
			{
				VKSdk.WakeUpSession(new string[] { offlineScope, emailScope }, (VKAuthorizationState state, Foundation.NSError error) =>
				{
					if (tcs.Task.Status == TaskStatus.RanToCompletion)
						return;

					if (state == VKAuthorizationState.Initialized || state == VKAuthorizationState.Authorized)
					{
						VKSdk.Authorize(new string[] { offlineScope, emailScope }, VKAuthorizationOptions.UnlimitedToken);
					}
					else if (state == VKAuthorizationState.Error)
					{
						tcs.TrySetException(new Exception());
					}
					else
						tcs.TrySetResult(null);
				});
			}
			catch (Exception ex)
			{
				VKSdk.Authorize(new string[] { offlineScope, emailScope }, VKAuthorizationOptions.UnlimitedToken);
			}

			return tcs.Task;
		}


		private class AppRopioVKSdkDelegate : VKSdkDelegate
		{
			public TaskCompletionSource<string> TCS { get; set; }

			public AppRopioVKSdkDelegate()
			{

			}

			public AppRopioVKSdkDelegate(TaskCompletionSource<string> tcs)
			{
				TCS = tcs;
			}

			public override void AccessTokenUpdated(VKAccessToken newToken, VKAccessToken oldToken)
			{
				if (newToken != null && oldToken != null)
				{
					if (!TCS.Task.IsCanceled)
						TCS.TrySetResult(newToken.AccessToken);
				}
			}

			public override void AccessAuthorizationFinished(VKAuthorizationResult result)
			{
				if (result.Error != null)
				{
					TCS.SetException(new Exception(result.Error.LocalizedDescription));
					return;
				}

				if (!TCS.Task.IsCanceled)
					TCS.TrySetResult(result.Token.AccessToken);
			}

			public override void UserAuthorizationFailed()
			{
				TCS.TrySetException(new OperationCanceledException());
			}
		}

		private class AppRopioVKSdkUiDelegate : VKSdkUIDelegate
		{
			public override void NeedCaptchaEnter(VKError captchaError)
			{
				//VKCaptchaViewController* vc = [VKCaptchaViewController captchaControllerWithError: captchaError];
				//[vc presentIn:self];   
			}

			public override void ShouldPresentViewController(UIViewController controller)
			{
				var masterNavigationController = ((MvxIosViewPresenter)Mvx.Resolve<IMvxIosViewPresenter>()).MasterNavigationController;
				var targetVC = GetLastPresentedController(masterNavigationController);
				targetVC.PresentViewController(controller, true, null);
			}

			private UIViewController GetLastPresentedController(UIViewController lastNotNullPresentedController)
			{
				return lastNotNullPresentedController.PresentedViewController != null ? GetLastPresentedController(lastNotNullPresentedController.PresentedViewController) : lastNotNullPresentedController;
			}
		}

		#endregion
	}
}

