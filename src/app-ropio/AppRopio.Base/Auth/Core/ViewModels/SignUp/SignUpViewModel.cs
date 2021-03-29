using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.Formatters;
using AppRopio.Base.Auth.Core.Messages.Registration;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Services;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp
{
    public class SignUpViewModel : AuthBaseViewModel, ISignUpViewModel
	{
		#region Fields

		private MvxSubscriptionToken _token;

		#endregion

		#region Commands

		private ICommand _signUpCommand;
		public ICommand SignUpCommand
		{
			get
			{
				return _signUpCommand ?? (_signUpCommand = new MvxCommand(OnSignUpExecute));
			}
		}

		#endregion

		#region Properties

		private List<ISignUpItemBaseViewModel> _items;
		public List<ISignUpItemBaseViewModel> Items
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

        public string SignUpText => LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignUp_Next");

		#endregion

		#region Services

		private ISignUpVmService _signUpService;
		protected ISignUpVmService SignUpService { get { return _signUpService ?? (_signUpService = Mvx.IoCProvider.Resolve<ISignUpVmService>()); } }

		#endregion

		#region Constructor

		public SignUpViewModel()
		{
			VmNavigationType = Base.Core.Models.Navigation.NavigationType.Push;
			Items = new List<ISignUpItemBaseViewModel>();

			_token = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<RegistrationItemTextChangedMessage>((RegistrationItemTextChangedMessage obj) =>
		   {
			   OnItemValueChanged(obj as ISignUpItemBaseViewModel);
		   });
		}


		#endregion

		#region Private

		private void SetItems()
		{
			Items = Mvx.IoCProvider.Resolve<ISignUpItemFactoryService>().ItemsFactory(Config.Items, Config.RequireConfirmPassword);
		}

		private bool CheckItems(bool markInvalidFields)
		{
			bool valid = true;

			foreach (var item in Items)
				if (!item.Internal)
				{
					bool valueValid;
					switch (item.RegistrationField.Type)
					{
						case AppRopio.Models.Auth.Enums.RegistrationFieldType.Email:
							valueValid = item.GetValue()?.IsMail() ?? false;
							break;
						case AppRopio.Models.Auth.Enums.RegistrationFieldType.Phone:
							valueValid = PhoneNumberFormatter.IsValid(item.GetValue());
							break;
						case AppRopio.Models.Auth.Enums.RegistrationFieldType.Password:
							valueValid = !item.GetValue().IsNullOrEmtpy();
							break;
						case AppRopio.Models.Auth.Enums.RegistrationFieldType.Date:
							valueValid = !item.Value.IsNullOrEmtpy();
							break;
						default:
							valueValid = !item.GetValue().IsNullOrEmtpy();
							break;
					}
					bool itemInvalid = !(valueValid || !item.RegistrationField.Required);
					if (markInvalidFields)
						item.Invalid = itemInvalid;

					if (itemInvalid)
					{
						valid = false;
					}
				}
				else
				{
					bool valueValid = true;
					switch (item.RegistrationField.Type)
					{
						case AppRopio.Models.Auth.Enums.RegistrationFieldType.Password:
							if (Config.RequireConfirmPassword)
							{
								var password = Items
								.FirstOrDefault(p =>
												   p.RegistrationField.Type == AppRopio.Models.Auth.Enums.RegistrationFieldType.Password
												  && p != item);

								valueValid = !item.GetValue().IsNullOrEmtpy() && password.GetValue() == item.GetValue();
							}

							break;
					}
					bool itemInvalid = !(valueValid || !item.RegistrationField.Required);
					if (markInvalidFields)
						item.Invalid = itemInvalid;

					if (itemInvalid)
					{
						valid = false;
					}
				}

			return valid;
		}

		#endregion

		#region Protected

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            SetItems();
        }

		/// <summary>
		/// Вызывается при изменении значения любого эл-та (Items[k].Value)
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void OnItemValueChanged(ISignUpItemBaseViewModel item)
		{
			PropertiesValid = CheckItems(false);
		}

		protected override bool IsViewModelPropertiesValid()
		{
			return CheckItems(true);
		}

		protected virtual async void OnSignUpExecute()
		{
			Loading = true;

			var check = await SignUpService.SignUp(Items, OnUnbindCTS);
			if (check != null)
			{
				if (check.Successful)
					NavigationVmService.NavigateToThanks(new BaseBundle(NavigationType.PresentModal));
				else
					Items.ForEach(p => p.Invalid = check.InvalidFieldsIds?.Contains(p.RegistrationField.Id) ?? false);
			}

			Loading = false;
		}

		#endregion

		#region Public

		public override void Unbind()
		{
			if (_token != null)
			{
				_token.Dispose();
				_token = null;
			}
			base.Unbind();
		}

		#endregion
	}
}
