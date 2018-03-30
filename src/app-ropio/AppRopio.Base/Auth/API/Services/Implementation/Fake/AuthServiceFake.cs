using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Models.Auth.Enums;
using AppRopio.Models.Auth.Responses;
using System.Linq;
using AppRopio.Models.Auth.Requests;
using Newtonsoft.Json;

namespace AppRopio.Base.Auth.API.Services.Implementation.Fake
{
	public class AuthServiceFake : IAuthService
	{
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
				//InvalidFields = new List<RegistrationFieldType>() {  RegistrationFieldType.Email, RegistrationFieldType.Password},
				Token = TOKEN
			};
			var email = fields.FirstOrDefault(p => p.Id == "1");
			if (email != null && email.Value.First() != 'a')
			{
				resp.Successful = false;
				resp.InvalidFieldsIds.Add("1");
				resp.Error += $"Поле с ID == 1 должно начинаться на \"a\"\n";
			}

			var password = fields.FirstOrDefault(p => p.Type == RegistrationFieldType.Password);
			if ((password?.Value?.Length ?? 0) < 6)
			{
				resp.Successful = false;
				resp.InvalidFieldsIds.Add(password.Id);
				resp.Error += $"Пароль должен быть не менее 6 символов\n";
			}
			resp.Error = resp.Error?.Trim();
			//if (password.Length <= 6)
			//{
			//	resp.Successful = false;
			//	resp.Error += "|Пароль| <= 6 ";
			//	resp.PasswordValid = false;
			//}
			//if (identifier.Length<3 || identifier[5]!='2')
			//{
			//	if ((resp.Error?.Length ?? 0) >0)
			//	{
			//		resp.Error += "\n";
			//	}
			//	resp.Successful = false;
			//	resp.Error += "ID[7]!=2 ";
			//	resp.IdentifierValid = false;
			//}
			return resp;
		}

		public async Task<string> SocialSignIn(string socialToken, SocialType socialType, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
			System.Diagnostics.Debug.WriteLine($"Выполнен вход в социальную сеть {socialType.ToString()}; Token :<{socialToken}>");
			return TOKEN;
		}

		public async Task ValidateCode(string code, CancellationTokenSource cancellationTokenSource)
		{
			await Task.Delay(1000);
		}
	}
}
