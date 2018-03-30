using AppRopio.Base.Core.Models.Bundle;
namespace AppRopio.Base.Core.Services.Router
{
    public interface IRouterSubscriber
    {
        bool CanNavigatedTo(string type, BaseBundle bundle);

        void FailedNavigatedTo(string type, BaseBundle bundle);
    }
}
 