using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppRopio.Payments.YandexKassa.Core.Services
{
    public interface IYandexKassaWebViewService
    {
        Task<bool> ProcessPayment(string url, string successUrl, string failUrl, HttpContent postContent);
    }
}