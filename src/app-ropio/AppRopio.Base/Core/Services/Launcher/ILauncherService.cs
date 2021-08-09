using System;
using System.Threading.Tasks;

namespace AppRopio.Base.Core.Services.Launcher
{
    public interface ILauncherService
    {
        Task<bool> LaunchPhone(string phoneNumber);

        Task<bool> LaunchUri(string uri);

        Task<bool> LaunchEmail(string email);

        Task LaunchAddress(string address);
    }
}