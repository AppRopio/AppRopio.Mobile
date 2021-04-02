using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using AppRopio.Base.Core;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Core.ViewModels._base;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public class CatalogItemVM : BaseViewModel, ICatalogItemVM
    {
        #region Commands

        private ICommand _markCommand;
        public ICommand MarkCommand
        {
            get
            {
                return _markCommand ?? (_markCommand = new MvxCommand(OnMarkedExecute));
            }
        }

        public ICommand ActionCommand { get; protected set; }

        #endregion

        #region Properties

        public Product Model { get; private set; }

        public string Id { get; protected set; }

        public string ImageUrl { get; protected set; }

        public string Name { get; protected set; }

        public string Price { get; protected set; }

        public string MaxPrice { get; protected set; }

        public string OldPrice { get; protected set; }

        public string StateName { get; protected set; }

        public List<IProductBadgeItemVM> Badges { get; protected set; }

        private bool _marked;
        public bool Marked
        {
            get
            {
                return _marked;
            }
            set
            {
                _marked = value;
                Model.IsMarked = value;

                RaisePropertyChanged(() => Marked);
            }
        }

        public bool MarkEnabled { get; }

        public string ActionText { get; protected set; }

        public bool HasAction { get; protected set; }

        public IMvxViewModel BasketBlockViewModel { get; protected set; }

        #endregion

        #region Services

        protected ICatalogVmService VmService => Mvx.IoCProvider.Resolve<ICatalogVmService>();

        protected IProductConfigService ConfigService => Mvx.IoCProvider.Resolve<IProductConfigService>();

        #endregion

        #region Constructor

        public CatalogItemVM(Product model)
        {
            Model = model;

            Id = model.Id;

            if (!model.ImageUrls.IsNullOrEmpty())
            {
                var image = model.ImageUrls.First();
                ImageUrl = image.SmallUrl;
            }

            Name = model.Name;

            Price = FormatPrice(model);

            MaxPrice = FormatMaxPrice(model);

            OldPrice = FormatOldPrice(model);

            if (!model.Badges.IsNullOrEmpty())
                Badges = model.Badges.Select(SetupBadgeItem).ToList();

            Marked = model.IsMarked;

            StateName = model.State?.Name;

            MarkEnabled = ConfigService.Config.MarkedEnabled;

            BasketBlockViewModel = VmService.LoadItemBasketVm();

            if (BasketBlockViewModel is IMvxViewModel<IMvxBundle> parameterVM)
                parameterVM.Prepare(new ProductCardBundle(Model, Base.Core.Models.Navigation.NavigationType.InsideScreen));
            else
                BasketBlockViewModel?.Init(new ProductCardBundle(Model, Base.Core.Models.Navigation.NavigationType.InsideScreen));

            if (BasketBlockViewModel is IActionCommandViewModel actionCommandVM) {
                HasAction = true;
                ActionCommand = actionCommandVM.ActionCommand;
                ActionText = actionCommandVM.ActionText;
            }
        }

        #endregion

        #region Private

        private string FormatPriceUnit(decimal price, string unitName)
        {
            string priceString = string.Empty;

            var numberFormat = (NumberFormatInfo)AppSettings.SettingsCulture.NumberFormat.Clone();
            var currencyFormat = AppSettings.CurrencyFormat;

            priceString += price.ToString(currencyFormat, numberFormat);

            var config = ConfigService.Config;

            if (config.UnitNameEnabled && !string.IsNullOrEmpty(unitName))
                priceString += $"/{unitName}";

            return priceString;
        }

        #endregion

        #region Protected

        protected virtual string FormatPrice(Product model)
        {
            decimal price = model.Price;
            decimal? maxPrice = model.MaxPrice;
            string unitName = model.UnitName;

            string priceString = FormatPriceUnit(price, unitName);

            var priceType = ConfigService.Config.PriceType;
            if (maxPrice.HasValue && priceType != PriceType.Default && priceType != PriceType.To)
                priceString = $"{LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Catalog_PriceFrom")} {priceString}";

            return priceString;
        }

        protected virtual string FormatOldPrice(Product model)
        {
            decimal? oldPrice = model.OldPrice;
            string unitNameOld = model.UnitNameOld;

            return oldPrice.HasValue ? FormatPriceUnit(oldPrice.Value, unitNameOld) : string.Empty;
        }

        protected virtual string FormatMaxPrice(Product model)
        {
            decimal? maxPrice = model.MaxPrice;

            if (!maxPrice.HasValue)
                return string.Empty;

            string unitName = model.UnitName;

            string priceString = $"{LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Catalog_PriceTo")} ";

            priceString += FormatPriceUnit(maxPrice.Value, unitName);

            return priceString;
        }

        protected virtual IProductBadgeItemVM SetupBadgeItem(ProductBadge model)
        {
            return new ProductBadgeItemVM(model);
        }

        protected virtual void OnMarkedExecute()
        {
            Marked = !Marked;

            Messenger.Publish(new ProductMarkChangedMessage(this, Model, Marked));
        }

        #endregion

        #region Public

        public override void Unbind()
        {
            (BasketBlockViewModel as IBaseViewModel)?.Unbind();

            base.Unbind();
        }

        #endregion
    }
}
