using System;
using System.Threading.Tasks;

namespace AppRopio.Base.Core.Services.Launcher
{
    public interface ILauncherService
    {
        Task LaunchPhone(string phoneNumber);

        Task LaunchUri(string uri);

        Task LaunchEmail(string email);

        Task LaunchAddress(string address);
    }
}