using AppRopio.Base.Auth.Core.Messages.Registration;
using AppRopio.Base.Auth.Core.Models.Registration;
using AppRopio.Models.Auth.Enums;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using AppRopio.Base.Core.Services.Localization;
using System;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Items
{
	public class SignUpItemBaseViewModel : MvxViewModel, ISignUpItemBaseViewModel
	{

		#region Fields

		#endregion

		#region Commands

		#endregion

		#region Properties

		public bool Internal { get; set; } = false;

		private RegistrationFieldType _type;
		public RegistrationFieldType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
				RaisePropertyChanged(() => Type);
			}
		}

		private RegistrationField _registrationField;
		public RegistrationField RegistrationField
		{
			set
			{
				_registrationField = value;
			}
			get
			{
				return _registrationField;
			}
		}

		private bool _invalid;
		public bool Invalid
		{
			get
			{
				return _invalid;
			}
			set
			{
				_invalid = value;
				RaisePropertyChanged(() => Invalid);
			}
		}

		private string _value;
		public string Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = value;
				RaisePropertyChanged(() => Value);
				Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new RegistrationItemTextChangedMessage(this));
				if (Invalid)
					Invalid = false;
			}
		}

		private string _placeholder;
		public string Placeholder
		{
			get
			{
				return _placeholder;
			}
			set
			{
				_placeholder = value;
				RaisePropertyChanged(() => Placeholder);
			}
		}

		#endregion

		#region Services

		#endregion

		#region Constructor

		public SignUpItemBaseViewModel()
		{
		}

		public SignUpItemBaseViewModel(RegistrationField field)
		{
			RegistrationField = field;
            Type = field.Type;
            Placeholder = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, $"{field.Type.ToString().ToFirstCharLowercase()}{field.Id}_placeholder");
		}

		#endregion

		#region Private

		#endregion

		#region Protected

		#endregion

		#region Public

		public virtual string GetValue()
		{
			return Value;
		}

		#endregion
	}
}
