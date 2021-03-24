using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Services;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.Marked.Core.ViewModels.Marked
{
    [Deeplink("marked")]
    public class MarkedViewModel : CatalogViewModel, IMarkedViewModel
    {
        #region Properties

        public override string NoResultsTitle => LocalizationService.GetLocalizableString(MarkedConstants.RESX_NAME, "EmptyTitle");

        public override string NoResultsText => LocalizationService.GetLocalizableString(MarkedConstants.RESX_NAME, "EmptyText");

        public override string CatalogTitle => LocalizationService.GetLocalizableString(MarkedConstants.RESX_NAME, "GoToCatalog");

        #endregion

        #region Services

        protected new IMarkedVmService VmService { get { return Mvx.Resolve<IMarkedVmService>(); } }

        #endregion

        #region Protected

        protected override void LoadHeaderVm()
        {

        }

        protected override async Task LoadItems()
        {
            Loading = true;

            var dataSource = await VmService.LoadMarkedProducts(LOADING_PRODUCTS_COUNT);

            InvokeOnMainThread(() =>
            {
                Items = dataSource;
                Empty = Items.IsNullOrEmpty();
            });

            CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_PRODUCTS_COUNT;
            LoadMoreCommand.RaiseCanExecuteChanged();

            Loading = false;
        }

        protected override void OnLoadMoreExecute()
        {
            if (Loading || LoadingMore)
                return;

            Task.Run(async () =>
            {
                LoadingMore = true;

                var dataSource = await VmService.LoadMarkedProducts(LOADING_PRODUCTS_COUNT, Items.Count);

                CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_PRODUCTS_COUNT;
                LoadMoreCommand.RaiseCanExecuteChanged();

                InvokeOnMainThread(() =>
                {
                    Items.AddRange(dataSource);
                    Empty = Items.IsNullOrEmpty();
                });

                LoadingMore = false;
            });
        }

        protected override void OnMarkChangedMsgRecieved(ProductMarkChangedMessage msg)
        {
            //обрабатываем только удаление товара
            if (msg.Marked)
                return;

            var product = msg.Model;

            InvokeOnMainThread(() =>
            {
                Items.Remove(Items.FirstOrDefault(p => p.Model.Id == product.Id &&
                                                             p.Model.GroupId == product.GroupId));
                Empty = Items.IsNullOrEmpty();
            });

            MarkProductVmService.MarkProductAsFavorite(msg, onSuccess: (groupId, productId, isMarked) => 
            {
                Messenger.Publish(new ProductMarkedQuantityChangedMessage(this));
            });
        }

        protected override void OnProductCardMarkedMessage(ProductCardMarkedMessage msg)
        {
            //обрабатываем только удаление товара
            if (msg.Marked)
                return;

            var product = msg.Model;

            InvokeOnMainThread(() =>
            {
                Items.Remove(Items.FirstOrDefault(p => p.Model.Id == product.Id &&
                                                             p.Model.GroupId == product.GroupId));
                Empty = Items.IsNullOrEmpty();
            });

        }

        protected override void OnCatalogExecute()
        {
            NavigationVmService.NavigateToMain(new BaseBundle(NavigationType.ClearAndPush));
        }

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<BaseBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(BaseBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;

            Title = LocalizationService.GetLocalizableString(MarkedConstants.RESX_NAME, "Title");
        }

        #endregion

        #endregion
    }
}
