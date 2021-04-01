using System;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay
{
    public interface IBest2PayViewModel : IBaseViewModel
    {
        IMvxCommand ShouldLoadCommand { get; }

        string PaymentUrl { get; }
    }
}