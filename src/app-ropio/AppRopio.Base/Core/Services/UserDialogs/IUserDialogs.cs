using System;
using System.Threading.Tasks;

namespace AppRopio.Base.Core.Services.UserDialogs
{
    public interface IUserDialogs
    {
        Task Alert(string message);

        Task Error(string message);

        Task<bool> Confirm(string message, string button, bool autoHide = false);
    }
}
