using System.Collections.Generic;
using AppRopio.Base.Auth.Core.Models.Registration;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Services
{
	public interface ISignUpItemFactoryService
	{
		/// <summary>
		/// Строит коллекцию полей ввода на основе конфига
		/// </summary>
		/// <returns>Коллекцию ISignUpItemBaseViewModel.</returns>
		/// <param name="fields">Поля из Config/Auth.json/Items</param>
		/// <param name="requireConfirmPassword">If set to <c>true</c> необходимо добавить поле подтверждения пароля, 
		/// которое проверяется на клиенте и НЕ передается на сервер.</param>
		List<ISignUpItemBaseViewModel> ItemsFactory(List<RegistrationField> fields, bool requireConfirmPassword);
	}
}
