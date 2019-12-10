using System;
using System.Threading.Tasks;
using AppRopio.Base.Map.Core.ViewModels.Points.Paged;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Map
{
    public class PointsMapViewModel : PointsCollectionVM, IPointsMapViewModel
    {
        #region Fields

        #endregion

        #region Commands

        #endregion

        #region Properties

        private bool _isShowInfoBlock;
        public bool IsShowInfoBlock
        {
            get
            {
                return _isShowInfoBlock;
            }
            set
            {
                _isShowInfoBlock = value;
                RaisePropertyChanged(() => IsShowInfoBlock);
            }
        }

        #endregion

        #region Services

        #endregion

        #region Constructor

        public PointsMapViewModel()
        {
            Title = LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "Map_Title");
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected override async Task LoadContent()
        {
            Loading = true;

            Items = await VmService.LoadPoints(null, 0, 100);

            Loading = false;
        }

        protected override void OnItemSelected(List.Items.IPointItemVM item)
        {
            base.OnItemSelected(item);

            IsShowInfoBlock = item?.Coordinates != null;
        }

        protected override Task ReloadContent()
        {
            return null;
        }

        protected override Task LoadMoreContent()
        {
            return null;
        }

        #endregion

        #region Public

        #endregion
    }
}
