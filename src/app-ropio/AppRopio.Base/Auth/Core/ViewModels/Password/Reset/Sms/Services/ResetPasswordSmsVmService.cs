﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.Base;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms.Services
{
    public class ResetPasswordSmsVmService : BaseVmService, IResetPasswordSmsVmService
    {

        #region Services

        protected IAuthService AuthService { get { return Mvx.IoCProvider.Resolve<IAuthService>(); } }


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
                Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                {
                    UserDialogs.Alert(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_CodeSent"));
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
