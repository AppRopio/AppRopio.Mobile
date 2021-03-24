using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo
{
    public class ShortInfoProductsPciVm : MvxViewModel, IShortInfoProductsPciVm
    {
        private IMvxCommand _markCommand;
        public IMvxCommand MarkCommand => _markCommand ?? (_markCommand = new MvxCommand(OnMarkExecute));

        private IMvxCommand _shareCommand;
        public IMvxCommand ShareCommand => _shareCommand ?? (_shareCommand = new MvxCommand(OnShareExecute, () => !ShareLoading));

        public string GroupId { get; }

        public string ProductId { get; }

        public string Name { get; }

        public decimal Price { get; }

        public string UnitName { get; }

        public decimal? OldPrice { get; }

        public string UnitNameOld { get; }

        public decimal? ExtraPrice { get; }

        public string UnitNameExtra { get; }

        public List<IProductBadgeItemVM> Badges { get; }

        public string StateName { get; }

        public bool IsPriceDependsOnParams { get; }

        private bool _marked;
        public bool Marked
        {
            get => _marked;
            set => SetProperty(ref _marked, value, nameof(Marked));
        }

        private bool _markedLoading;
        public bool MarkedLoading
        {
            get => _markedLoading;
            set => SetProperty(ref _markedLoading, value, nameof(MarkedLoading));
        }

        private bool _shareLoading;
        public bool ShareLoading
        {
            get => _shareLoading;
            set => SetProperty(ref _shareLoading, value, nameof(ShareLoading));
        }

        public ShortInfoProductsPciVm(Product model)
        {
            GroupId = model.GroupId;
            ProductId = model.Id;

            Marked = model.IsMarked;

            IsPriceDependsOnParams = !GroupId.IsNullOrEmpty() && ProductId.IsNullOrEmpty();

            Name = model.Name;

            Price = model.Price;
            UnitName = model.UnitName;

            OldPrice = model.OldPrice;
            UnitNameOld = model.UnitNameOld;

            ExtraPrice = model.ExtraPrice?.Value;
            UnitNameExtra = model.ExtraPrice?.UnitName;

            if (!model.Badges.IsNullOrEmpty())
                Badges = model.Badges.Select(SetupBadgeItem).ToList();

            StateName = model.State?.Name;
        }

        protected virtual IProductBadgeItemVM SetupBadgeItem(ProductBadge model)
        {
            return new ProductBadgeItemVM(model);
        }

        protected virtual void OnMarkExecute()
        {
            if (!MarkedLoading)
            {
                MarkedLoading = true;
                //MarkCommand.RaiseCanExecuteChanged();

                Marked = !Marked;

                Task.Run(async () =>
                {
                    var result = await Mvx.Resolve<IMarkProductVmService>().MarkProductAsFavorite(GroupId, ProductId, Marked);
                    if (!result)
                        InvokeOnMainThread(() => Marked = !Marked);

                    InvokeOnMainThread(() =>
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new ProductCardMarkedMessage(this, new Product { GroupId = GroupId, Id = ProductId }, Marked));

                        MarkedLoading = false;
                        //MarkCommand.RaiseCanExecuteChanged();
                    });
                });
            }
        }

        protected virtual void OnShareExecute()
        {
            ShareLoading = true;

            ShareCommand.RaiseCanExecuteChanged();

            Task.Run(async () =>
            {
                await Mvx.Resolve<IProductsShareVmService>().ShareProduct(GroupId, ProductId);

                InvokeOnMainThread(() =>
                {
                    ShareLoading = false;
                    ShareCommand.RaiseCanExecuteChanged();
                });
            });
        }
    }
}
