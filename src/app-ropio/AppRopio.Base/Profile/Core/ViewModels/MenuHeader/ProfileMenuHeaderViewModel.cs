using System.Windows.Input;
using AppRopio.Base.Auth.Core.Messages.Session;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Profile.Core.Services;
using AppRopio.Base.Profile.Core.ViewModels.Profile;
using AppRopio.Models.Auth.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Profile.Core.ViewModels.MenuHeader
{
    public class ProfileMenuHeaderViewModel : BaseViewModel
    {
        #region Fields

        private MvxSubscriptionToken _userTypeChangedSubscriptionToken;

        private MvxSubscriptionToken _userInfoChangedSubscriptionToken;

        protected IProfileVmNavigationService NavigationVmService { get { return Mvx.Resolve<IProfileVmNavigationService>(); } }

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand(OnSelectionChangedExecute));
            }
        }

        #endregion

        #region Properties

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private string _userPhotoUrl;
        public string UserPhotoUrl
        {
            get
            {
                return _userPhotoUrl;
            }
            set
            {
                _userPhotoUrl = value;
                RaisePropertyChanged(() => UserPhotoUrl);
            }
        }

        private bool _userSignedIn;
        public bool UserSignedIn
        {
            get
            {
                return _userSignedIn;
            }
            set
            {
                _userSignedIn = value;
                RaisePropertyChanged(() => UserSignedIn);
            }
        }

        #endregion

        #region Services

        #endregion

        #region Constructor

        public ProfileMenuHeaderViewModel()
        {
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() =>
            {
                _userTypeChangedSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<UserTypeChangedMessage>(OnUserTypeChanged);
                _userInfoChangedSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<UserInfoChangedMessage>(OnUserInfoChanged);
            });

            Mvx.CallbackWhenRegistered<ISessionService>(() =>
            {
                UserSignedIn = Mvx.Resolve<ISessionService>().Alive;
                if (UserSignedIn)
                    OnCurrentProfileChanged(Mvx.Resolve<ISessionService>().GetUser());
            });

            TrackInAnalytics = false;
        }

        #endregion

        #region Private

        private void OnUserTypeChanged(UserTypeChangedMessage obj)
        {
            UserSignedIn = obj.UserType == Auth.Core.Models.UserType.UserType.Registered;
        }

        private void OnUserInfoChanged(UserInfoChangedMessage obj)
        {
            OnCurrentProfileChanged(obj.User);
        }

        #endregion

        #region Protected


        protected virtual void OnCurrentProfileChanged(User currentUser)
        {
            UserName = currentUser.Name;
            UserPhotoUrl = currentUser.PhotoUrl;
        }

        protected virtual void OnSelectionChangedExecute()
        {
            if (UserSignedIn)
                NavigationVmService.NavigateToProfile(new BaseBundle(NavigationType.ClearAndPush));
            else
                NavigationVmService.NavigateToAuthorization(new BaseBundle(NavigationType.PresentModal));
        }

        #endregion

        #region Public

        public override void Unbind()
        {
            base.Unbind();
            if (_userInfoChangedSubscriptionToken != null)
            {
                _userInfoChangedSubscriptionToken.Dispose();
                _userInfoChangedSubscriptionToken = null;
            }
            if (_userTypeChangedSubscriptionToken != null)
            {
                _userTypeChangedSubscriptionToken.Dispose();
                _userTypeChangedSubscriptionToken = null;
            }
        }

        #endregion
    }
}
