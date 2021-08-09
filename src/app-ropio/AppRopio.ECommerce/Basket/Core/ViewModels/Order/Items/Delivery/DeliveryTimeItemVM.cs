using System;
using MvvmCross.ViewModels;
using AppRopio.Models.Basket.Responses.Order;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public class DeliveryTimeItemVM : MvxViewModel, IDeliveryTimeItemVM
    {
        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region Constructor

        public DeliveryTimeItemVM(DeliveryTime model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;
            Name = model.Name;
        }

        #endregion

        #region Public

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
