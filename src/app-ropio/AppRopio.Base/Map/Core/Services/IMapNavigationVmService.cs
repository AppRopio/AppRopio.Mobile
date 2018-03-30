using AppRopio.Base.Map.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Base.Map.Core.Services
{
    public interface IMapNavigationVmService
    {
        void NavigateToPointAdditionalInfo(PointBundle bundle);

        void NavigateToMap(BaseBundle bundle);
    }
}
