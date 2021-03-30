using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Profile.Core.ViewModels.MenuHeader;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Ios.Views;
using ObjCRuntime;
using UIKit;
using MvvmCross;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Profile.Core;
using FFImageLoading.Cross;

namespace AppRopio.Base.Profile.iOS.View.MenuHeader
{
    public partial class ProfileMenuHeaderView : MvxView, IMvxIosView<ProfileMenuHeaderViewModel>
    {
        #region Properties

        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (ProfileMenuHeaderViewModel)value;
            }
        }

        private MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
                ViewModel = (ProfileMenuHeaderViewModel)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public ProfileMenuHeaderViewModel ViewModel { get; set; }

        #endregion

        #region Constructor

        public ProfileMenuHeaderView()
        {
            InitializeControls();
            this.DelayBind(BindControls);
        }

        public ProfileMenuHeaderView(IntPtr handle)
            : base(handle)
        {

        }

        #endregion

        #region Protected

        #region InitializationControls

        private void InitializeControls()
        {
            Frame = GetFrame();

            var arr = NSBundle.MainBundle.LoadNib(nameof(ProfileMenuHeaderView), null, null);
            var viewFromNib = Runtime.GetNSObject<ProfileMenuHeaderView>(arr.ValueAt(0));

            _userName = viewFromNib._userName;
            _userPhoto = viewFromNib._userPhoto;
            _helpLabel = viewFromNib._helpLabel;
            _signLabel = viewFromNib._signLabel;
            _selectionChangedBtn = viewFromNib._selectionChangedBtn;
            _cornersView = viewFromNib._cornersView;

            SetupSignLabel(_signLabel);
            SetupUserName(_userName);
            SetupHelpLabel(_helpLabel);
            SetupCornerView(_cornersView);

            viewFromNib.Frame = Bounds;
            viewFromNib.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            AddSubview(viewFromNib);
        }

        protected virtual CGRect GetFrame()
        {
            return new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 138);
        }

        protected virtual void SetupSignLabel(UILabel label)
        {
            label.TextColor = (UIColor)Theme.ColorPalette.TextMenu;
        }

        protected virtual void SetupUserName(UILabel label)
        {
            label.TextColor = (UIColor)Theme.ColorPalette.TextMenu;
        }

        protected virtual void SetupHelpLabel(UILabel label)
        {
            label.TextColor = (UIColor)Theme.ColorPalette.TextMenu;
        }

        protected virtual void SetupCornerView(UIView view)
        {
            view.Layer.BorderColor = ((UIColor)Theme.ColorPalette.TextMenu).CGColor;
            view.Layer.BorderWidth = 1;
        }

        #endregion

        #region BindingControls

        private void BindControls()
        {
            var set = this.CreateBindingSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel>();

            BindUserImage(_userPhoto, set);
            BindUserName(_userName, set);
            BindCornerView(_cornersView, set);
            BindHelpLabel(_helpLabel, set);
            BindSignInLabel(_signLabel, set);
            BindSelectionChangedBtn(_selectionChangedBtn, set);

            set.Apply();
        }

        protected void BindSelectionChangedBtn(UIButton selectionChangedBtn, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            set.Bind(selectionChangedBtn).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual void BindCornerView(UIView cornersView, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            set.Bind(cornersView).For("Visibility").To(vm => vm.UserSignedIn).WithConversion("Visibility");
        }

        protected virtual void BindUserImage(UIImageView imageView, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            if (imageView is MvxCachedImageView mvxImageView)
            {
                set.Bind(mvxImageView).For(i => i.ImagePath).To(vm => vm.UserPhotoUrl);
            }
            set.Bind(imageView).For("Visibility").To(vm => vm.UserSignedIn).WithConversion("Visibility");
        }

        protected virtual void BindUserName(UILabel label, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            set.Bind(label).To(vm => vm.UserName);
            set.Bind(label).For("Visibility").To(vm => vm.UserSignedIn).WithConversion("Visibility");
        }

        protected virtual void BindHelpLabel(UILabel label, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            set.Bind(label).For("Visibility").To(vm => vm.UserSignedIn).WithConversion("Visibility");
            set.Bind(label).To(vm => vm.Help);
        }

        protected virtual void BindSignInLabel(UILabel label, MvxFluentBindingDescriptionSet<ProfileMenuHeaderView, ProfileMenuHeaderViewModel> set)
        {
            set.Bind(label).For("Visibility").To(vm => vm.UserSignedIn).WithConversion("InvertedVisibility");
            set.Bind(label).To(vm => vm.SignIn);
        }

        #endregion

        #endregion
    }
}
