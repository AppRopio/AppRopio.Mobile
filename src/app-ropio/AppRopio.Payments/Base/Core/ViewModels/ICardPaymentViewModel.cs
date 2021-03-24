using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Payments.Core.Services;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.Core.ViewModels
{
    public interface ICardPaymentViewModel : IBaseViewModel
    {
        IMvxCommand PayCommand { get; }

		string CardNumber { get; set; }

		string ExpirationDate { get; set; }

		string Cvv { get; set; }

		string CardHolder { get; set; }

		bool CanGoNext { get; }
    }
}