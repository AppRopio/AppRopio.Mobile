using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal
{
    public class HorizontalCollectionPciVm : BaseCollectionPciVm<CollectionItemVM, ProductParameterValue>, IHorizontalCollectionPciVm
    {
        protected virtual ProductsConfig Config { get { return Mvx.Resolve<IProductConfigService>().Config; } }

        public HorizontalCollectionPciVm(ProductParameter parameter)
            : base(parameter)
        {
            Values = parameter.Values;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Items = new System.Collections.ObjectModel.ObservableCollection<CollectionItemVM>(Items.OrderByDescending(x => x.Selected));

            if (Config.ProductDetails.SelectFirstHorizontal)
            {
                bool selected = Items?.Any(x => x.Selected) ?? false;
                if (!selected)
                {
                    var firstItem = Items?.FirstOrDefault();
                    if (firstItem != null)
                        OnItemSelected(firstItem);
                }
            }
        }

        protected override void OnItemSelected(CollectionItemVM item)
        {
            var selectedItem = Items.FirstOrDefault(x => x.Selected);
            if (selectedItem != null && selectedItem.Id != item.Id)
                selectedItem.Selected = false;

            if (!item.Selected)
            {
                Items.Remove(item);

                item.Selected = true;

                Items.Insert(0, item);

                Task.Run(() => BuildSelectedValue());
            }
        }

        protected override CollectionItemVM SetupItem(ProductDataType dataType, ProductParameterValue value)
        {
            return new CollectionItemVM(dataType, value);
        }

		protected override Task BuildSelectedValue(bool withNotifyPropertyChanged = true)
		{
			if (CTS != null)
				CTS.Cancel(false);

			CTS = new CancellationTokenSource();

			return Task.Run(() =>
			{
				_selectedValue = new ApplyedProductParameter
				{
					Id = this.Id,
					DataType = this.DataType,
					Values = Items.Where(x => x.Selected).Select(y => new ApplyedProductParameterValue { Id = y.Id }).ToList()
				};

                if (withNotifyPropertyChanged)
                    RaisePropertyChanged(() => SelectedValue);
                
			}, CTS.Token);
		}

		public override void ClearSelectedValue()
		{
			Task.Run(() =>
			{
				SelectedValue = null;

				Items.ForEach(x => InvokeOnMainThread(() => x.Selected = false));
			});
		}
    }
}
