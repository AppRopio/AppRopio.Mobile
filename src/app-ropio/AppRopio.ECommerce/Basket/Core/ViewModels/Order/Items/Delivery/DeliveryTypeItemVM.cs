using System;
using AppRopio.Models.Basket.Responses.Enums;
using MvvmCross.Core.ViewModels;
using DeliveryModel = AppRopio.Models.Basket.Responses.Order.Delivery;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public class DeliveryTypeItemVM : MvxViewModel, IDeliveryTypeItemVM
    {
        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        public DeliveryType Type { get; private set; }

        public decimal? Price { get; private set; }

        public bool IsDeliveryTimeRequired { get; private set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public bool IsRequiredDataEntry { get; }

        public bool NotAvailable { get; }

        public string Message { get; }

        #endregion

        #region Constructor

        public DeliveryTypeItemVM(DeliveryModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            
            Id = model.Id;
            Name = model.Name;
            Type = model.Type;
            Price = model.Price;
            IsRequiredDataEntry = model.RequiredDataEntry;
            IsDeliveryTimeRequired = model.DeliveryTimeIsNeeded;
            NotAvailable = model.NotAvailable;
            Message = model.Message;
            IsSelected = model.PreviouslySelected;
        }

        #endregion
    }
}
