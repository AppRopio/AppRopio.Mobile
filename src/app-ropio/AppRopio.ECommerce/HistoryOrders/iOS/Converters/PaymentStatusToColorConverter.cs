using System;
using System.Globalization;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Platform.Converters;
using UIKit;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Converters
{
	public class PaymentStatusToColorConverter : MvxValueConverter<PaymentStatus, UIColor>
	{
        public UIColor PaidColor { get; set; }

        public UIColor NotPaidColor { get; set; }

		protected override UIColor Convert(PaymentStatus value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case PaymentStatus.Paid:
                    return PaidColor;

				case PaymentStatus.NotPaid:
                    return NotPaidColor;
				default:
                    return UIColor.Clear;
			}
		}
	}
}