using System.Collections.Generic;
using AppRopio.Base.Auth.Core.Models.Registration;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Items
{
	public class SignUpItemPickerViewModel : SignUpItemBaseViewModel
	{
		#region Properties

		private List<string> _items = new List<string>();
		public List<string> Items
		{
			get
			{
				return _items;
			}
			set
			{
				_items = value;
				RaisePropertyChanged(() => Items);
			}
		}

		#endregion

		public SignUpItemPickerViewModel(RegistrationField field) : base(field)
		{
			Items = field.Values;
		}

		public SignUpItemPickerViewModel()
		{

		}
	}
}
