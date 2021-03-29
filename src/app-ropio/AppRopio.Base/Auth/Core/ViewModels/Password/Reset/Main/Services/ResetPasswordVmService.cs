using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main.Services
{
    public class ResetPasswordVmService : BaseVmService, IResetPasswordVmService
    {

        #region Services

        protected IAuthService AuthService { get { return Mvx.IoCProvider.Resolve<IAuthService>(); } }

        #endregion

        #region IResetMainVmService implementation

        public async Task<bool> ForgotPassword(string identifier, CancellationTokenSource cts)
        {
            try
            {
                await AuthService.ForgotPassword(identifier, cts);

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

        #endregion
    }
}
