using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public class DeliveryPointOnMapVM : BaseViewModel, IDeliveryPointOnMapVM
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

        private IDeliveryPointItemVM _item;
        public IDeliveryPointItemVM Item
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

        public DeliveryPointOnMapVM()
        {
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var deliveryPointBundle = parameters.ReadAs<DeliveryPointBundle>();
            this.InitFromBundle(deliveryPointBundle);
        }

        protected virtual void InitFromBundle(DeliveryPointBundle deliveryPointBundle)
        {
            Item = new DeliveryPointItemVM(deliveryPointBundle);
            VmNavigationType = deliveryPointBundle.NavigationType;
        }

        protected virtual async Task OnCloseExecute()
        {
            await NavigationVmService.Close(this);
        }

        #endregion

        #region Public

        #endregion
    }
}
