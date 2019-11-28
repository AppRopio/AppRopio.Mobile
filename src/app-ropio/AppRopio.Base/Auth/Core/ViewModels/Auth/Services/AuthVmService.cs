using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.Core.Extentions;
using AppRopio.Base.Auth.Core.Models.OAuth;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Auth.Core.ViewModels.Auth.Services
{
    public class AuthVmService : BaseVmService, IAuthVmService
    {
        #region Services

        protected IOAuthService OAuthService { get { return Mvx.Resolve<IOAuthService>(); } }

        protected IAuthService AuthService { get { return Mvx.Resolve<IAuthService>(); } }

        protected IUserService UserService { get { return Mvx.Resolve<IUserService>(); } }

        #endregion

        #region Protected

        protected virtual async Task<bool> OnSignInBySocial(string socialToken, OAuthType socialType, CancellationTokenSource cts)
        {
            if (socialToken.IsNullOrEmtpy())
                throw new Exception(Mvx.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Auth_Social_NoAccess"));
            try
            {
                var token = await AuthService.SocialSignIn(socialToken, socialType.GetSocialType(), cts);

                AuthSettings.Token = token;

                return await Mvx.Resolve<ISessionService>().StartByToken(token);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex, Mvx.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Auth_Social_Failed"));
            }

            return false;
        }

        #endregion

        #region ISocialVmService implementation

        public async Task SignInTo(OAuthType socialType, string cancelledError, string error, CancellationTokenSource cts)
        {
            try
            {
                var socialToken = await OAuthService.SignInTo(socialType);

                if (await OnSignInBySocial(socialToken, socialType, cts))
                {
                    await UserDialogs.Alert(Mvx.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Auth_Social_Success"));
                    ChangePresentation(new Base.Core.PresentationHints.NavigateToDefaultViewModelHint());
                }
            }
            catch (OperationCanceledException)
            {
                await UserDialogs.Alert(cancelledError);
            }
            catch (Exception ex)
            {
                OnException(ex, error);
            }
        }

        #endregion
    }
}
