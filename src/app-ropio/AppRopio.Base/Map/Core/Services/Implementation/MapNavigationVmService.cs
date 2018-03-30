using AppRopio.Base.Map.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Core.Models.Bundle;
using System;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;

namespace AppRopio.Base.Map.Core.Services.Implementation
{
    public class MapNavigationVmService : BaseVmNavigationService, IMapNavigationVmService
    {
        #region IMapNavigationVmService implementation

        public void NavigateToPointAdditionalInfo(PointBundle bundle)
        {
            NavigateTo<IPointAdditionalInfoVM>(bundle);
        }

        public void NavigateToMap(BaseBundle bundle)
        {
            NavigateTo<IPointsMapViewModel>(bundle);
        }

        #endregion
    }
}
