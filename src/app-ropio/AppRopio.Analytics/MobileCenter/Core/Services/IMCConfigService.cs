using System;
namespace AppRopio.Analytics.MobileCenter.Core.Services
{
    public interface IMCConfigService
    {
        string AppCenterKey_IOS { get; }

        string AppCenterKey_DROID { get; }
    }
}
