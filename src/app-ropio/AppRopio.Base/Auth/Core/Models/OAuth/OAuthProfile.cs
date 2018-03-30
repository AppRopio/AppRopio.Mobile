using System;
namespace AppRopio.Base.Auth.Core.Models.OAuth
{
    /// <summary>
    /// Результат авторизации через OAuth
    /// </summary>
    public class OAuthProfile
    {
        /// <summary>
        /// Токен, получаемый при успешной авторизации и использующийся в дальнейшем для повторной авторизации и доступа к данным
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// E-mail пользователя, авторизовавшегося через OAuth
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Тип авторизации через OAuth
        /// </summary>
        public OAuthType OAuthType { get; set; }

        /// <summary>
        /// Имя, полученное от соц.сети
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия, полученная от соц.сети
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Ссылка на фотографию пользователя к соц. сетия
        /// </summary>
        /// 
        public string PhotoUrl { get; set; }
    }
}
