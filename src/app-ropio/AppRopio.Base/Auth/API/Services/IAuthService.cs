using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Models.Auth.Enums;
using AppRopio.Models.Auth.Requests;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.API.Services
{
	public interface IAuthService
	{
		/// <summary>
		/// Авторизация пользователя по идентификатору и паролю
		/// </summary>
		/// <returns>Авторизационный токен пользователя</returns>
		/// <param name="identifier">Идентификатор</param>
		/// <param name="password">Пароль</param>
		Task<string> SignIn(string identifier, string password, CancellationTokenSource cancellationTokenSource);

		/// <summary>
		/// Авторизация пользователя по токену соц. сети и её типу
		/// </summary>
		/// <returns>Авторизационный токен пользователя</returns>
		/// <param name="socialToken">Токен соцсети</param>
		/// <param name="socialType">Тип соцсети</param>
		Task<string> SocialSignIn(string socialToken, SocialType socialType, CancellationTokenSource cancellationTokenSource);

		/// <summary>
		///  Регистрация пользователю
		/// </summary>
		/// <returns>регистрационный токен пользователя.</returns>
		/// <param name="fields">Коллекция полей.</param>
		/// <param name="cancellationTokenSource">Cancellation token source.</param>
		Task<RegistrationResponse> SignUp(List<RegistrationRequestItem> fields, CancellationTokenSource cancellationTokenSource);

		/// <summary>
		/// Восстановление забытого пароля
		/// </summary>
		/// <param name="identifier">идентификатору</param>
		Task ForgotPassword(string identifier, CancellationTokenSource cancellationTokenSource);

		/// <summary>
		/// Метод повторной отправки кода подтверждения пользователю.
		/// </summary>
		Task ResendCode(CancellationTokenSource cancellationTokenSource);

		/// <summary>
		/// Подтверждение кода, полученнного из смс
		/// </summary>
		/// <returns>Авторизационный токен пользователя</returns>
		/// <param name="code">Проверочный код</param>
		Task ValidateCode(string code, CancellationTokenSource cancellationTokenSource);

		/// <summary>
		/// Задание нового пароля
		/// </summary>
		/// <returns>Авторизационный токен пользователя.</returns>
		/// <param name="password">Пароль.</param>
		Task<string> NewPassword(string password, CancellationTokenSource cancellationTokenSource);
	}
}
