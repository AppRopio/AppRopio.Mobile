using System;
using AppRopio.Base.Auth.Core.Models.Config;
namespace AppRopio.Base.Auth.Core.Services
{
    public interface IAuthConfigService
    {
        AuthConfig Config { get; }
    }
}
