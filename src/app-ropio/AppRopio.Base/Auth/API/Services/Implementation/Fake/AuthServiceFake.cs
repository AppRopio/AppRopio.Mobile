using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Auth.Enums;
using AppRopio.Models.Auth.Requests;
using AppRopio.Models.Auth.Responses;
using MvvmCross;

namespace AppRopio.Base.Auth.API.Services.Implementation.Fake
{
    public class AuthServiceFake : IAuthService
	{
        public bool IsRussianCulture => Mvx.IoCProvider.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");
        
		private const string TOKEN = "auth_token_from_fake_service";

		public async Task ForgotPassword(string identifier, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
		}

		public async Task<string> NewPassword(string password, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
			return TOKEN;
		}

		public async Task ResendCode(CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(800);
		}

		public async Task<string> SignIn(string identifier, string password, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
			return TOKEN;
		}

		public async Task<RegistrationResponse> SignUp(List<RegistrationRequestItem> fields, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);

			var resp = new RegistrationResponse()
			{
				Successful = true,
				Error = "",
				InvalidFieldsIds = new List<string>(),
				Token = TOKEN
			};
			var email = fields.FirstOrDefault(p => p.Id == "1");
			if (email != null && email.Value.First() != 'a')
			{
				resp.Successful = false;
				resp.InvalidFieldsIds.Add("1");
                resp.Error += IsRussianCulture ? $"Поле с ID == 1 должно начинаться на \"a\"\n" : "E-mail should starts with \"a\" letter\n";
			}

			var password = fields.FirstOrDefault(p => p.Type == RegistrationFieldType.Password);
			if ((password?.Value?.Length ?? 0) < 6)
			{
				resp.Successful = false;
				resp.InvalidFieldsIds.Add(password.Id);
                resp.Error += IsRussianCulture ? $"Пароль должен быть не менее 6 символов\n" : "Password length couldn't be less than 6";
			}
			resp.Error = resp.Error?.Trim();

			return resp;
		}

		public async Task<string> SocialSignIn(string socialToken, SocialType socialType, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
            System.Diagnostics.Debug.WriteLine(IsRussianCulture ? $"Выполнен вход в социальную сеть {socialType.ToString()}; Token :<{socialToken}>" : $"You successfully authorized with {Enum.GetName(typeof(SocialType), socialType)}");
			return TOKEN;
		}

		public async Task ValidateCode(string code, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
		}
	}
}
