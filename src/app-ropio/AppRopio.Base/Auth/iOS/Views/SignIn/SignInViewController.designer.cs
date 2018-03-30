// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.SignIn
{
	[Register ("SignInViewController")]
	partial class SignInViewController
	{
		[Outlet]
		UIKit.UIStackView _bottomStackView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _descriptionLabel { get; set; }

		[Outlet]
		UIKit.UIView _headerView { get; set; }

		[Outlet]
		UIKit.UIImageView _iconImage { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARTextField _identityField { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _imageHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _imageWidth { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARTextField _passwordField { get; set; }

		[Outlet]
		UIKit.UIButton _resetPasswordButton { get; set; }

		[Outlet]
		UIKit.UIButton _signInButton { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

		[Outlet]
		UIKit.UIStackView _topStackView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_bottomStackView != null) {
				_bottomStackView.Dispose ();
				_bottomStackView = null;
			}

			if (_descriptionLabel != null) {
				_descriptionLabel.Dispose ();
				_descriptionLabel = null;
			}

			if (_headerView != null) {
				_headerView.Dispose ();
				_headerView = null;
			}

			if (_iconImage != null) {
				_iconImage.Dispose ();
				_iconImage = null;
			}

			if (_identityField != null) {
				_identityField.Dispose ();
				_identityField = null;
			}

			if (_imageHeight != null) {
				_imageHeight.Dispose ();
				_imageHeight = null;
			}

			if (_imageWidth != null) {
				_imageWidth.Dispose ();
				_imageWidth = null;
			}

			if (_passwordField != null) {
				_passwordField.Dispose ();
				_passwordField = null;
			}

			if (_resetPasswordButton != null) {
				_resetPasswordButton.Dispose ();
				_resetPasswordButton = null;
			}

			if (_signInButton != null) {
				_signInButton.Dispose ();
				_signInButton = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_topStackView != null) {
				_topStackView.Dispose ();
				_topStackView = null;
			}
		}
	}
}
