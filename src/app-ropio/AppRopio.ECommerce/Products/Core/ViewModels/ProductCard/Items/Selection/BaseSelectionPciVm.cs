using System;
using System.Collections.Generic;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using AppRopio.Models.Products.Responses;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Models.Bundle;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection
{
    public abstract class BaseSelectionPciVm : ProductDetailsItemVM, IBaseSelectionPciVm
    {
        #region Fields

        private MvxSubscriptionToken _subscribtionToken;

        #endregion

        #region Properties

        protected List<ProductParameterValue> Values { get; private set; }

        protected List<ApplyedProductParameterValue> ApplyedFilterValues { get; set; }

        #endregion

        #region Services

        protected new IProductsNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IProductsNavigationVmService>(); } }

        #endregion

        #region Constructor

        protected BaseSelectionPciVm(ProductParameter parameter)
            : base(parameter)
        {
            Values = parameter.Values;
            ApplyedFilterValues = new List<ApplyedProductParameterValue>();
        }

        #region Protected

        protected abstract void OnSelectionMessageReceived(ProductDetailsSelectionChangedMessage msg);

        #endregion

        #endregion

        #region Public

        #region IBaseSelectionFiVm implementation

        public void OnSelected()
        {
            if (_subscribtionToken == null)
                _subscribtionToken = Messenger.Subscribe<ProductDetailsSelectionChangedMessage>(OnSelectionMessageReceived);

            NavigationVmService.NavigateToSelection(new SelectionBundle(this.Id, this.Name, this.WidgetType, Values, SelectedValue?.Values));
        }

        #endregion

        #endregion
    }
}
