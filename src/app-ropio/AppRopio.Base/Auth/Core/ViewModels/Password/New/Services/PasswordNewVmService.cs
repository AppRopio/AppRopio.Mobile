using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.Platform.Core;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.New.Services
{
    public class PasswordNewVmService : BaseVmService, IPasswordNewVmService
    {

        #region Services

        protected IAuthService AuthService { get { return Mvx.Resolve<IAuthService>(); } }

        #endregion

        #region IPasswordNewVmService implementation

        public async Task<bool> SetNewPassword(string password, CancellationTokenSource cts)
        {
            try
            {
                await AuthService.NewPassword(password, cts);

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
