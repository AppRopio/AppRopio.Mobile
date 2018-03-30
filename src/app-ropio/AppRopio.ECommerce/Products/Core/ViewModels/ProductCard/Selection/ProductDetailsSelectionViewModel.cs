using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels.Selection;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Core.ViewModels.Selection.Services;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection
{
    public class ProductDetailsSelectionViewModel : BaseSelectionViewModel<ProductParameterValue, ApplyedProductParameterValue>, IProductDetailsSelectionViewModel
    {
        #region Fields

        private string _parameterId;

        private ProductWidgetType _parameterWidgetType;

        #endregion

        #region Properties

        protected CancellationTokenSource CTS { get; private set; }

        #endregion

        #region Services

        protected override IBaseSelectionVmService<ProductParameterValue, ApplyedProductParameterValue> VmService => Mvx.Resolve<IProductDetailsSelectionVmService>();

        #endregion

        #region Private

        protected override async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadItemsFor(Values, SelectedValues, SearchText);

            if (_parameterWidgetType == ProductWidgetType.MultiSelection)
                dataSource = new ObservableCollection<ISelectionItemVM>(dataSource.OrderByDescending(x => x.Selected));

            Items = dataSource;

            Loading = false;
        }

        private Task BuildSelectedValues()
        {
            if (CTS != null)
                CTS.Cancel(false);

            CTS = new CancellationTokenSource();

            return Task.Run(() =>
            {
                SelectedValues = Items.Where(x => x.Selected).Select(y => new ApplyedProductParameterValue { Id = y.Id }).ToList();
            }, CTS.Token);
        }

        #endregion

        #region Protected

        protected override void OnItemSelected(ISelectionItemVM item)
        {
            if (_parameterWidgetType == ProductWidgetType.MultiSelection)
                item.Selected = !item.Selected;
            else
            {
                Items.ForEach(x => x.Selected = false);
                item.Selected = true;
            }

            Task.Run(BuildSelectedValues);
        }

        protected override void OnApplyExecute()
        {
            (VmService as IProductDetailsSelectionVmService).ChangeSelectedParameterValuesTo(_parameterId, SelectedValues);

            Close(this);
        }

        protected override void OnClearExecute()
        {
            SelectedValues = null;

            Task.Run(LoadContent);
        }

        #region Init

        public override void Prepare(MvvmCross.Core.ViewModels.IMvxBundle parameters)
        {
            var sortParameters = parameters.ReadAs<SelectionBundle>();
            this.InitFromBundle(sortParameters);
        }

        protected virtual void InitFromBundle(SelectionBundle parameters)
        {
            _parameterId = parameters.ParameterId;
            _parameterWidgetType = parameters.WidgetType;

            Name = parameters.ParameterName;
            Values = parameters.Values;
            SelectedValues = parameters.SelectedValues;
        }

        #endregion

        #region Search

        protected override void OnSearchTextChanged(string searchText)
        {
            //nothing
        }

        protected override void SearchCommandExecute()
        {
            Task.Run(LoadContent);
        }

        protected override void CancelSearchExecute()
        {
            SearchText = null;

            Task.Run(LoadContent);
        }

        #endregion

        #endregion

    }
}
