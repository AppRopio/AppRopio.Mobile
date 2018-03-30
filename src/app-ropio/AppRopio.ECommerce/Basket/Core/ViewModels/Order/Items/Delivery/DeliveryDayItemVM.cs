using System;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using AppRopio.Models.Basket.Responses.Order;
using System.Linq;
namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public class DeliveryDayItemVM : MvxViewModel, IDeliveryDayItemVM
    {
        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        public List<IDeliveryTimeItemVM> Times { get; private set; }

        #endregion

        #region Constructor

        public DeliveryDayItemVM(DeliveryDay model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;
            Name = model.Name;
            Times = model.Times.Select(SetupDeliveryTimeItems).ToList();
        }

        #endregion

        #region Protected

        protected virtual IDeliveryTimeItemVM SetupDeliveryTimeItems(DeliveryTime model)
        {
            return new DeliveryTimeItemVM(model);
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
