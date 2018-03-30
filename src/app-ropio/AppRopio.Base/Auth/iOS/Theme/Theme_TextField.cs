using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.iOS
{
	public static class AuthTheme
	{
		private static int size = 40;
		public static void SetupSecurity(this UITextField view)
		{
			UIImageView eyeImage = new UIImageView();

			eyeImage.SetupStyle(Mvx.Resolve<IAuthThemeConfigService>().ThemeConfig.SecureTextImage);

			eyeImage.ChangeFrame(0, 0, size, size);
			eyeImage.ContentMode = UIViewContentMode.Center;
			eyeImage.Highlighted = !view.SecureTextEntry;

			var eyeButton = new UIButton()
				.WithBackground(UIColor.Clear)
				.WithFrame(new CoreGraphics.CGRect(0, 0, 40, 40))
				.WithSubviews(eyeImage);
			eyeButton.TouchUpInside += (sender, e) =>
			{
				view.SecureTextEntry = !view.SecureTextEntry;
				eyeImage.Highlighted = !view.SecureTextEntry;
			};

			view.RightView = eyeButton;
			view.RightViewMode = UITextFieldViewMode.Always;
		}
	}
}
