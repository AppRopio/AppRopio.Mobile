using AppRopio.Base.Auth.Core.Models.Registration;
using AppRopio.Models.Auth.Enums;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Items
{
	public interface ISignUpItemBaseViewModel : IMvxViewModel
	{
		RegistrationField RegistrationField { get; }

		bool Invalid { get; set; }

		/// <summary>
		/// Дублирует значение RegistrationField.Type 
		/// </summary>
		/// <value>The type.</value>
		RegistrationFieldType Type { get; }

		string Value { get; }

		string Placeholder { get; }

		/// <summary>
		/// Показывает то, что элемент используется только на клиенте и не передается на сервер
		/// <see cref="T:AppRopio.Base.Auth.Core.ViewModels.SignUp.Items.ISignUpItemViewModel"/> is internal.
		/// </summary>
		/// <value><c>true</c> if internal; otherwise, <c>false</c>.</value>
		bool Internal { get; }

		/// <summary>
		/// Возвращает значени, которое будет отправлено на сервер
		/// </summary>
		/// <returns>The value.</returns>
		string GetValue();
	}
}
