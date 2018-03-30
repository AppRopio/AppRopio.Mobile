using System;
using AppRopio.Base.Auth.Core.Models.Config;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views._base
{
	public abstract class AuthBaseViewController<T> : CommonViewController<T>
	   where T : class, IAuthBaseViewModel
	{
		#region Fields

		protected UIButton _accessoryButton;

		protected bool _lastKeyboardShownState;

		#endregion

		#region Properties

		protected Models.AuthThemeConfig ThemeConfig { get { return Mvx.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

		protected AuthConfig Config { get { return Mvx.Resolve<IAuthConfigService>().Config; } }

		protected abstract string AccessoryButtonTitle { get; }

		protected virtual nfloat StandardAnimationDuration
		{
			get
			{
				return 0.27f;
			}
		}

		protected virtual nfloat ShortAnimationDuration
		{
			get
			{
				return 0.15f;
			}
		}

		#endregion

		#region Constructor

		protected AuthBaseViewController()
		{

		}

		protected AuthBaseViewController(IntPtr handle)
			: base(handle)
		{

		}

		protected AuthBaseViewController(string nibName, Foundation.NSBundle bundle)
			: base(nibName, bundle)
		{

		}

		#endregion

		#region Protected

		protected abstract void OnNextButtonClick(object sender, EventArgs e);

		protected virtual void SetupAccessoryButton()
		{
			_accessoryButton = new UIButton()
				.WithTitleForAllStates(AccessoryButtonTitle)
				.WithFrame(new CoreGraphics.CGRect(0, 0, DeviceInfo.ScreenWidth, 44f));
			_accessoryButton.SetupStyle(ThemeConfig.AccessoryButton);
			_accessoryButton.TouchUpInside += OnNextButtonClick;
		}

		protected override void InitializeControls()
		{
			SetupAccessoryButton();
		}

		protected override void CleanUp()
		{

			if (_accessoryButton != null)
			{
				_accessoryButton.TouchUpInside -= OnNextButtonClick;
				_accessoryButton.RemoveFromSuperview();
				_accessoryButton.Dispose();
				_accessoryButton = null;
			}

			base.CleanUp();
		}

		#endregion

	}
}
