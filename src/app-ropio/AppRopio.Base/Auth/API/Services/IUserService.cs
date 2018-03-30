using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Загрузка информации о пользователе
        /// </summary>
        /// <returns>Модель пользователя <see cref="AppRopio.Base.Auth.API.Models.User"/></returns>
        /// <param name="token">Token.</param>
        Task<User> GetUserBy(string token);

        /// <summary>
        /// Изменение данных в одном поле модели пользователя
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="data">Новые данные</param>
        Task ChangeUsersData(string fieldName, object data);

        /// <summary>
        /// Изменение данных в нескольких полях модели пользователя
        /// </summary>
        /// <param name="fields">Измененные поля в формате имя-данные</param>
        Task ChangeUserData(Dictionary<string, object> fields);
    }
}
