// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.Password.Reset.Main
{
	[Register ("ResetPasswordViewController")]
	partial class ResetPasswordViewController
	{
		[Outlet]
		UIKit.UIView _bottomView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _descriptionLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _emailDescriptionLabel { get; set; }

		[Outlet]
		UIKit.UIButton _emailDoneButton { get; set; }

		[Outlet]
		UIKit.UIImageView _emailIconImage { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _emailImageHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _emailImageWidth { get; set; }

		[Outlet]
		UIKit.UIView _emailSendedView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _emailTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton _getPasswordButton { get; set; }

		[Outlet]
		UIKit.UIView _headerIconImageView { get; set; }

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
		UIKit.UIStackView _mainStackView { get; set; }

		[Outlet]
		UIKit.UIView _spacingView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_headerIconImageView != null) {
				_headerIconImageView.Dispose ();
				_headerIconImageView = null;
			}

			if (_bottomView != null) {
				_bottomView.Dispose ();
				_bottomView = null;
			}

			if (_descriptionLabel != null) {
				_descriptionLabel.Dispose ();
				_descriptionLabel = null;
			}

			if (_emailDescriptionLabel != null) {
				_emailDescriptionLabel.Dispose ();
				_emailDescriptionLabel = null;
			}

			if (_emailDoneButton != null) {
				_emailDoneButton.Dispose ();
				_emailDoneButton = null;
			}

			if (_emailIconImage != null) {
				_emailIconImage.Dispose ();
				_emailIconImage = null;
			}

			if (_emailImageHeight != null) {
				_emailImageHeight.Dispose ();
				_emailImageHeight = null;
			}

			if (_emailImageWidth != null) {
				_emailImageWidth.Dispose ();
				_emailImageWidth = null;
			}

			if (_emailSendedView != null) {
				_emailSendedView.Dispose ();
				_emailSendedView = null;
			}

			if (_emailTitleLabel != null) {
				_emailTitleLabel.Dispose ();
				_emailTitleLabel = null;
			}

			if (_getPasswordButton != null) {
				_getPasswordButton.Dispose ();
				_getPasswordButton = null;
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

			if (_mainStackView != null) {
				_mainStackView.Dispose ();
				_mainStackView = null;
			}

			if (_spacingView != null) {
				_spacingView.Dispose ();
				_spacingView = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}
		}
	}
}
