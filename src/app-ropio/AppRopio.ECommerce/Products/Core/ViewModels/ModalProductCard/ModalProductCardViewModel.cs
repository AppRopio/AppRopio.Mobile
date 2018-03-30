using System;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard
{
    public class ModalProductCardViewModel : ProductCardViewModel, IModalProductCardViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _closeCommand;
        public IMvxCommand CloseCommand => _closeCommand ?? (_closeCommand = new MvxCommand(OnCloseExecute));

        #endregion

        #region Properties

        #endregion

        #region Services

        #endregion

        #region Constructor

        public ModalProductCardViewModel()
        {

        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #region Init

        protected override void InitFromBundle(Models.Bundle.ProductCardBundle parameters)
        {
            parameters.NavigationType = Base.Core.Models.Navigation.NavigationType.PresentModal;

            base.InitFromBundle(parameters);
        }

        #endregion

        protected virtual void OnCloseExecute()
        {
            Close(this);
        }

        #endregion
    }
}
