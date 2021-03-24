using System.Net.Http;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa
{
    public interface IYandexKassaViewModel : IBaseViewModel
    {
        IMvxCommand ShouldLoadCommand { get; }

        string PaymentUrl { get; set; }

        HttpContent PaymentParams { get; set; }
    }
}