using System;
namespace AppRopio.Base.API.Services
{
    /// <summary>
    /// Сервис для логгирования ошибок в приложении на сервере
    /// </summary>
    public interface IErrorService
    {
        /// <summary>
        /// Отправляет ошибку на сервер
        /// </summary>
        void Send(string message, string stackTrace, string packageName, string appVersion, string deviceName, byte[] data = null);
    }
}
