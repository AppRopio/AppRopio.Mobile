using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms.Services
{
    public class ResetPasswordSmsVmService : BaseVmService, IResetPasswordSmsVmService
    {

        #region Services

        protected IAuthService AuthService { get { return Mvx.Resolve<IAuthService>(); } }


        #endregion

        #region IResetSmsVmService implementation

        public async Task<bool> VerifyCode(string code, CancellationTokenSource cts)
        {
            try
            {
                await AuthService.ValidateCode(code, cts);

                return true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);

                return false;
            }
            catch (Exception ex)
            {
                OnException(ex);

                return false;
            }
        }

        public async Task ResendCode(CancellationTokenSource cts)
        {
            try
            {
                await AuthService.ResendCode(cts);
                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                {
                    UserDialogs.Alert("Код подтверждения отправлен.");
                });
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);

            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        #endregion
    }
}
