using System;
using System.Collections.Generic;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.ViewModels;
using System.Linq;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public class OrderFieldsGroupVM : MvxViewModel, IOrderFieldsGroupVM
    {
        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        public List<IOrderFieldItemVM> Items { get; private set; }

        #endregion

        #region Constructor

        public OrderFieldsGroupVM(OrderFieldsGroup model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;
            Name = model.Name;
            Items = model.Items.Select(SetupOrderFieldItem).ToList();
        }

        #endregion

        #region Protected

        protected virtual IOrderFieldItemVM SetupOrderFieldItem(OrderField model)
        {
            return new OrderFieldItemVM(model);
        }

        #endregion
    }
}
