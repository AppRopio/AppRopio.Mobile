using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Auth.Enums;
using AppRopio.Models.Auth.Requests;
using AppRopio.Models.Auth.Responses;
using MvvmCross;

namespace AppRopio.Base.Auth.API.Services.Implementation
{
    public class AuthService : BaseService, IAuthService
    {
        #region Fields

        protected const string SIGN_IN_URL = "auth/";
        protected const string SIGN_IN_SOCIAL_URL = "socialauth/";
        protected const string SIGN_UP_URL = "register/";
        protected const string FORGOT_PASSWORD_URL = "auth/forgotpassword/";
        protected const string VALIDATE_CODE_URL = "auth/validatecode/";
        protected const string NEW_PASSWORD_URL = "auth/newpassword/";
        protected const string RESEND_CODE_URL = "auth/resendcode/";

        #endregion

        #region IAuthService implementation

        public async Task<string> SignIn(string identifier, string password, CancellationTokenSource cancellationTokenSource)
        {
            return await Post<string>(SIGN_IN_URL, ToStringContent(new AuthRequest() { Identifier = identifier, Password = password }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task<string> SocialSignIn(string socialToken, SocialType socialType, CancellationTokenSource cancellationTokenSource)
        {
            return await Post<string>(SIGN_IN_SOCIAL_URL, ToStringContent(new SocialAuthRequest() { Token = socialToken, Type = socialType }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task<RegistrationResponse> SignUp(List<RegistrationRequestItem> fields, CancellationTokenSource cancellationTokenSource)
        {
            return await Post<RegistrationResponse>(SIGN_UP_URL, ToStringContent(new RegistrationRequest() { Fields = fields }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task ForgotPassword(string identifier, CancellationTokenSource cancellationTokenSource)
        {
            await Post(FORGOT_PASSWORD_URL, ToStringContent(new ForgotPasswordRequest() { Identifier = identifier }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task ValidateCode(string code, CancellationTokenSource cancellationTokenSource)
        {
            await Post(VALIDATE_CODE_URL, ToStringContent(new ValidateCodeRequest() { Code = code }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task<string> NewPassword(string password, CancellationTokenSource cancellationTokenSource)
        {
            return await Post<string>(NEW_PASSWORD_URL, ToStringContent(new NewPasswordRequest() { Password = password }), cancellationToken: cancellationTokenSource.Token);
        }

        public async Task ResendCode(CancellationTokenSource cancellationTokenSource)
        {
            await Get<string>(RESEND_CODE_URL, cancellationToken: cancellationTokenSource.Token);
        }


        #endregion
    }
}
