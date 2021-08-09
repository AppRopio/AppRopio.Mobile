using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Map.Core.Models.Bundle;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points
{
    public class PointAdditionalInfoVM : BaseViewModel, IPointAdditionalInfoVM
    {
        #region Fields

        #endregion

        #region Commands

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxAsyncCommand(OnCloseExecute));
            }
        }

        #endregion

        #region Properties

        private IPointItemVM _item;
        public IPointItemVM Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }

        #endregion

        #region Services

        #endregion

        #region Constructor

        public PointAdditionalInfoVM()
        {

        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var deliveryPointBundle = parameters.ReadAs<PointBundle>();
            this.InitFromBundle(deliveryPointBundle);
        }

        protected virtual void InitFromBundle(PointBundle deliveryPointBundle)
        {
            Item = new PointItemVM(deliveryPointBundle);
            VmNavigationType = deliveryPointBundle.NavigationType;
        }

        #endregion

        protected virtual async Task OnCloseExecute()
        {
            await NavigationVmService.Close(this);
        }

        #endregion

        #region Public

        #endregion
    }
}
