using System;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public class DeliveryPointAdditionalInfoVM : BaseViewModel, IDeliveryPointAdditionalInfoVM
    {
        #region Fields

        #endregion

        #region Commands

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
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

        public DeliveryPointAdditionalInfoVM()
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

            var deliveryPointBundle = parameters.ReadAs<DeliveryPointBundle>();
            this.InitFromBundle(deliveryPointBundle);
        }

        protected virtual void InitFromBundle(DeliveryPointBundle deliveryPointBundle)
        {
            Item = new DeliveryPointItemVM(deliveryPointBundle);
            VmNavigationType = deliveryPointBundle.NavigationType;
        }

        #endregion

        #endregion

        #region Public

        #endregion
    }
}
