using System;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks
{
    public class ThanksViewModel : BaseViewModel, IThanksViewModel
    {
		#region Commands

		private ICommand _goToCatalogCommand;
		public ICommand GoToCatalogCommand
		{
			get
			{
                return _goToCatalogCommand ?? (_goToCatalogCommand = new MvxCommand(OnClose));
			}
		}

		private ICommand _closeCommand;
		public ICommand CloseCommand
		{
			get
			{
                return _closeCommand ?? (_closeCommand = new MvxCommand(OnClose));
			}
		}

        #endregion

        private string _orderId;

        public string OrderId
        {
            get { return _orderId; }
            set
            {
                SetProperty(ref _orderId, value);
            }
        }

        #region Init

        public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<ThanksBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(ThanksBundle parameters)
		{
			VmNavigationType = parameters.NavigationType == NavigationType.None ?
															NavigationType.ClearAndPush :
															parameters.NavigationType;

            OrderId = parameters.OrderId;
		}

		#endregion

        protected virtual void OnClose()
        {
            Close(this);
        }
	}
}