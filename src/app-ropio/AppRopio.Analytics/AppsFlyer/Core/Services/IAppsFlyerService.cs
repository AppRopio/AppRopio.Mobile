using System;
namespace AppRopio.Analytics.AppsFlyer.Core.Services
{
    public interface IAppsFlyerService
    {
        void TrackScreen(string screenName);

        void TrackEvent(string category, string action, string label, object model);

        void TrackECommerce(decimal fullPrice, float quantity, string orderId, string currency);

        void TrackException(string message, bool isFatal);
    }
}
