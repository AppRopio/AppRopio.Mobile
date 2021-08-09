using System.Collections.Generic;
using AppRopio.Base.Auth.Core.Models.Registration;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Services
{
	public class SignUpItemFactoryService : BaseVmService, ISignUpItemFactoryService
	{
		public SignUpItemFactoryService()
		{
		}

		#region ISignUpItemFactoryService implementation

		public List<ISignUpItemBaseViewModel> ItemsFactory(List<RegistrationField> fields, bool requireConfirmPassword)
		{
			var result = new List<ISignUpItemBaseViewModel>();

			foreach (var item in fields)
			{
				switch (item.Type)
				{
					case AppRopio.Models.Auth.Enums.RegistrationFieldType.Date:
						result.Add(new SignUpItemDatePickerViewModel(item));
						break;

					case AppRopio.Models.Auth.Enums.RegistrationFieldType.Picker:
						result.Add(new SignUpItemPickerViewModel(item));
						break;

					case AppRopio.Models.Auth.Enums.RegistrationFieldType.Password:
						result.Add(new SignUpItemBaseViewModel(item));
						if (requireConfirmPassword)
						{
							var confirmPassword = new SignUpItemBaseViewModel(item)
							{
								Internal = true,
                                Placeholder = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "ConfirmPassword")
							};
							result.Add(confirmPassword);
						}
						break;

					default:
						result.Add(new SignUpItemBaseViewModel(item));
						break;
				}

			}
			return result;
		}

		#endregion
	}
}
