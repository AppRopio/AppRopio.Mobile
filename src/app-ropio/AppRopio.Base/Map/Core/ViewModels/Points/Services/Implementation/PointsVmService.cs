using System;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using AppRopio.Models.Map.Responses;
using AppRopio.Base.Map.API.Services;
using MvvmCross;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using System.Linq;
using AppRopio.Models.Base.Responses;
using MvvmCross.ViewModels;
using AppRopio.Base.Core.Services.Location;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Services.Implementation
{
    public class PointsVmService : BaseVmService, IPointsVmService
    {
        #region Protected

        protected virtual IPointItemVM SetupPointItem(Point model)
        {
            return new PointItemVM(model);
        }

        #endregion

        #region Services

        protected IPointsService PointsService => Mvx.IoCProvider.Resolve<IPointsService>();

        protected ILocationService LocationService => Mvx.IoCProvider.Resolve<ILocationService>();

        #endregion

        #region IPointsVmService implementation

        public async Task<MvxObservableCollection<IPointItemVM>> LoadPoints(string searchText, int offset = 0, int count = 10)
        {
            MvxObservableCollection<IPointItemVM> source = null;

            try
            {
                var position = await LocationService.GetCurrentLocation();

                var deliveryPoints = await PointsService.GetPoints(position, searchText, offset, count);

                source = new MvxObservableCollection<IPointItemVM>(deliveryPoints.Select(SetupPointItem));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return source;
        }

        #endregion
    }
}
