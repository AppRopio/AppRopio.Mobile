using System;
using MvvmCross.ViewModels;
using PaymentModel = AppRopio.Models.Basket.Responses.Order.Payment;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items
{
    public class PaymentItemVM : MvxViewModel, IPaymentItemVM
    {
        public PaymentModel Payment { get; private set; }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

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

        public PaymentItemVM(PaymentModel payment)//, bool isSelected = false)
        {
            Payment = payment;
            Title = payment.Name;
            IsSelected = payment.PreviouslySelected;//isSelected;
        }
    }
}
