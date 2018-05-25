using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using AppRopio.ECommerce.Basket.Core.Services;
using MvvmCross.Droid.Views;

namespace AppRopio.ECommerce.Basket.Droid.Views.Basket
{
    public class BasketFragment : CommonFragment<IBasketViewModel>
    {
        public BasketFragment()
            : base (Resource.Layout.app_basket_basket)
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_Title");
        }

        protected virtual void SetupRecyclerView(View view, MvxRecyclerView recyclerView) => recyclerView.Adapter = SetupAdapter(recyclerView);

        protected virtual IMvxRecyclerAdapter SetupAdapter(MvxRecyclerView recyclerView) => new ARSimpleTuneRecyclerAdapter((IMvxAndroidBindingContext)BindingContext)
        {
            TuneViewHolderOnBind = TuneItemOnBind
        };

        protected virtual void TuneItemOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var moreButton = viewHolder.ItemView.FindViewById<ImageButton>(Resource.Id.app_basket_basket_item_moreButton);
            if (moreButton != null)
            {
                moreButton.SetOnClickListener(new OnMoreClickListener(Context, (viewHolder as IMvxRecyclerViewHolder).DataContext));
            }
        }

        protected virtual void SetupLoyaltyViewIfExist(LinearLayout basketButtonLayout)
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            var productConfigService = Mvx.Resolve<IBasketConfigService>();

            var config = productConfigService.Config;

            if (config.Loyalty != null && viewLookupService.IsRegistered(config.Loyalty.TypeName))
            {
                var loyaltyView = ViewModel.LoyaltyVm == null ? null : Activator.CreateInstance(viewLookupService.Resolve(config.Loyalty.TypeName), Context) as IMvxAndroidView;
                if (loyaltyView != null)
                {
                    loyaltyView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(LayoutInflater), ViewModel.LoyaltyVm);

                    basketButtonLayout.AddView((View)loyaltyView, 0);
                }
            }
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_basket_basket_recyclerView);
            SetupRecyclerView(view, recyclerView);

            var basketButtonLayout = view.FindViewById<LinearLayout>(Resource.Id.app_basket_basket_buttonLayout);
            SetupLoyaltyViewIfExist(basketButtonLayout);
        }

        private class OnMoreClickListener : Java.Lang.Object, View.IOnClickListener, Android.Support.V7.Widget.PopupMenu.IOnMenuItemClickListener
        {
            readonly object _dataContext;
            readonly Context _context;

            public OnMoreClickListener(Context context, object dataContext)
            {
                _context = context;
                _dataContext = dataContext;
            }

            public void OnClick(View v)
            {
                var popupMenu = new Android.Support.V7.Widget.PopupMenu(_context, v);
                popupMenu.Menu.Add(0, 0, 0, new Java.Lang.String(Mvx.Resolve<ILocalizationService>().GetLocalizableString(BasketConstants.RESX_NAME, "Basket_DeleteItem")));
                popupMenu.SetOnMenuItemClickListener(this);
                popupMenu.Show();
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                if (_dataContext is IBasketItemVM basketItem)
                    basketItem.DeleteCommand.Execute(null);

                return true;
            }
        }
    }
}
