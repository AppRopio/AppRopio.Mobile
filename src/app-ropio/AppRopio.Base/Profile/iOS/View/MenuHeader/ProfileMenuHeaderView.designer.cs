// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.Base.Profile.iOS.View.MenuHeader
{
	[Register("ProfileMenuHeaderView")]
	partial class ProfileMenuHeaderView
	{
		[Outlet]
		UIKit.UIView _cornersView { get; set; }


		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _helpLabel { get; set; }


		[Outlet]
		UIKit.UIButton _selectionChangedBtn { get; set; }


		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _signLabel { get; set; }


		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _userName { get; set; }


		[Outlet]
		UIKit.UIImageView _userPhoto { get; set; }

		void ReleaseDesignerOutlets()
		{
			if (_cornersView != null)
			{
				_cornersView.Dispose();
				_cornersView = null;
			}

			if (_helpLabel != null)
			{
				_helpLabel.Dispose();
				_helpLabel = null;
			}

			if (_selectionChangedBtn != null)
			{
				_selectionChangedBtn.Dispose();
				_selectionChangedBtn = null;
			}

			if (_signLabel != null)
			{
				_signLabel.Dispose();
				_signLabel = null;
			}

			if (_userName != null)
			{
				_userName.Dispose();
				_userName = null;
			}

			if (_userPhoto != null)
			{
				_userPhoto.Dispose();
				_userPhoto = null;
			}
		}
	}
}
