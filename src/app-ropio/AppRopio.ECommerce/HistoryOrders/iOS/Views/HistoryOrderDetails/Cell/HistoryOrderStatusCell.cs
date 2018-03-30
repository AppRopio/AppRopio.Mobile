using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrderStatusCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HistoryOrderStatusCell");
        public static readonly UINib Nib;

		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

		static HistoryOrderStatusCell()
        {
            Nib = UINib.FromName("HistoryOrderStatusCell", NSBundle.MainBundle);
        }

		protected HistoryOrderStatusCell(IntPtr handle) : base(handle)
        {
			this.DelayBind(() =>
			{
				InitializeControls();
				BindControls();
			});
		}

		#region InitializationControls

		protected virtual void InitializeControls()
		{
            TextLabel.SetupStyle(ThemeConfig.HistoryOrderStatusCell.Title);

            this.SetupStyle(ThemeConfig.HistoryOrderStatusCell);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<HistoryOrderStatusCell, string>();

            BindStatus(TextLabel, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindStatus(UILabel status, MvxFluentBindingDescriptionSet<HistoryOrderStatusCell, string> set)
		{
			set.Bind(status).To(vm => vm);
		}

		#endregion
	}
}