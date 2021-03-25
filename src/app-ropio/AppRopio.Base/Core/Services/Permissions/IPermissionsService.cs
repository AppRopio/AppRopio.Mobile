using System.Threading.Tasks;

namespace AppRopio.Base.Core.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<bool> CheckPermission(Xamarin.Essentials.Permissions.BasePermission permission, bool goToSettingsAlert = false, string goToSettingsMessage = null);
    }
}
