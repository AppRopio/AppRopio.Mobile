using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Auth.Requests;
using AppRopio.Models.Auth.Responses;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Services
{
    public class SignUpVmService : BaseVmService, ISignUpVmService
    {
        #region Services

        protected IAuthService AuthService { get { return Mvx.Resolve<IAuthService>(); } }

        #endregion

        #region ISignUpService implementation

        public async Task<RegistrationResponse> SignUp(List<ISignUpItemBaseViewModel> fields, CancellationTokenSource cts)
        {
            try
            {
                var requestFields = new List<RegistrationRequestItem>();
                fields.ForEach(p =>
                {
                    if (!p.Internal)
                        requestFields.Add(new RegistrationRequestItem()
                        {
                            Type = p.RegistrationField.Type,
                            Value = p.GetValue(),
                            Id = p.RegistrationField.Id
                        });
                });
                var result = await AuthService.SignUp(requestFields, cts);
                if (result == null || !result.Successful)
                {
                    await UserDialogs.Alert(result?.Error ?? string.Empty);
                }
                else
                {
                    if (result.Successful)
                        await StartSession(result.Token);
                }

                return result;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);

                return null;
            }
            catch (Exception ex)
            {
                OnException(ex);

                return null;
            }
        }

        public async Task<bool> StartSession(string token)
        {
            try
            {
                AuthSettings.Token = token;
                await Mvx.Resolve<ISessionService>().StartByToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
