using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Auth.Responses;
using MvvmCross;

namespace AppRopio.Base.Auth.API.Services.Implementation.Fake
{
    public class UserServiceFake : IUserService
    {
        public bool IsRussianCulture => Mvx.IoCProvider.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        public async Task ChangeUserData(Dictionary<string, object> fields)
        {
            await Task.Delay(300);
        }

        public async Task ChangeUsersData(string fieldName, object data)
        {
            await Task.Delay(300);
        }

        public async Task<User> GetUserBy(string token)
        {
            await Task.Delay(300);
            return new User
            { 
                Name = IsRussianCulture ? "Константин" : "John",
                PhotoUrl = @"https://thumbs.dreamstime.com/z/vector-boy-avatar-man-face-expression-icon-linear-avatars-75578693.jpg"
            };
        }
    }
}
