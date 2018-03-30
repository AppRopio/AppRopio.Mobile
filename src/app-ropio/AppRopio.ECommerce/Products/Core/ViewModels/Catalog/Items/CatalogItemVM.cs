using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using System.Linq;
using System;
using System.Windows.Input;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.ECommerce.Products.Core.Messages;
using System.Collections.Generic;
using AppRopio.ECommerce.Products.Core.Services;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public class CatalogItemVM : MvxViewModel, ICatalogItemVM
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

        #endregion

        #region Services

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

            OldPrice = model.OldPrice;
            UnitNameOld = model.UnitNameOld;

            if (!model.Badges.IsNullOrEmpty())
                Badges = model.Badges.Select(SetupBadgeItem).ToList();

            Marked = model.IsMarked;

            StateName = model.State?.Name;

            MarkEnabled = ConfigService.Config.MarkedEnabled;
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
    }
}
