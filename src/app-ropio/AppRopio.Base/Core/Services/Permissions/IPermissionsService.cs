using System;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;

namespace AppRopio.Base.Core.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<bool> CheckPermission(Permission permission, bool goToSettingsAlert = false, string goToSettingsMessage = null);
    }
}
