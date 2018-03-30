using System;
using System.Threading.Tasks;

namespace AppRopio.Payments.YandexKassa.API.Services
{
    public interface IYandexKassaService
    {
        Task DsrpWallet(string shopId, string orderId, decimal amount, string currency, string paymentData);
    }
}