using System;
using AppRopio.Base.Auth.Core.Models.Registration;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Items
{
	public class SignUpItemDatePickerViewModel : SignUpItemBaseViewModel
	{
		#region Properties

		private DateTime _selectedDate;
		public DateTime SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			set
			{
				_selectedDate = value;
				RaisePropertyChanged(() => SelectedDate);
			}
		}

		private DateTime? _minDate;
		public DateTime? MinDate
		{
			get
			{
				return _minDate;
			}
			set
			{
				_minDate = value;
				RaisePropertyChanged(() => MinDate);
			}
		}

		private DateTime? _maxDate;
		public DateTime? MaxDate
		{
			get
			{
				return _maxDate;
			}
			set
			{
				_maxDate = value;
				RaisePropertyChanged(() => MaxDate);
			}
		}

		#endregion

		#region Constructor

		public SignUpItemDatePickerViewModel()
		{
		}

		public SignUpItemDatePickerViewModel(RegistrationField field) : base(field)
		{
			MaxDate = field.MaxDate;
			MinDate = field.MinDate;
		}

		#endregion

		#region Public

		public override string GetValue()
		{
			return SelectedDate.ToString("s");
		}

		#endregion
	}
}
