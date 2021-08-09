using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.ViewModels.SignIn.Services
{
    public class SignInVmService : BaseVmService, ISignInVmService
    {

        #region Services

        protected IAuthService AuthService { get { return Mvx.IoCProvider.Resolve<IAuthService>(); } }

        #endregion

        #region ISignInService implementation

        public async Task<bool> SignIn(string identifier, string password, CancellationTokenSource cts)
        {
            try
            {
                var token = await AuthService.SignIn(identifier, password, cts);

                return await Mvx.IoCProvider.Resolve<ISessionService>().StartByToken(token);
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
