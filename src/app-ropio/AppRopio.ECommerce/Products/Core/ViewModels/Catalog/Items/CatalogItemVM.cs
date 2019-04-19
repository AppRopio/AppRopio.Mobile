using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels;

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

        #endregion

        #region Properties

        public Product Model { get; private set; }

        public string Id { get; protected set; }

        public string ImageUrl { get; protected set; }

        public string Name { get; protected set; }

        public decimal Price { get; protected set; }

        public decimal? MaxPrice { get; protected set; }

        public string UnitName { get; protected set; }

        public decimal? OldPrice { get; protected set; }

        public string UnitNameOld { get; protected set; }

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


        public IMvxViewModel BasketBlockViewModel { get; protected set; }

        #endregion

        #region Services

        protected ICatalogVmService VmService => Mvx.Resolve<ICatalogVmService>();

        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected IProductConfigService ConfigService => Mvx.Resolve<IProductConfigService>();

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

            Price = model.Price;
            UnitName = model.UnitName;

            MaxPrice = model.MaxPrice;

            OldPrice = model.OldPrice;
            UnitNameOld = model.UnitNameOld;

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
        }

        #endregion

        #region Protected

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
