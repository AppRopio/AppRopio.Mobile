using System;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Controls;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Views;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Droid.Views.ProductCard
{
    public class ProductCardFragment : ProductCardFragment<IProductCardViewModel>
    {
        protected const int MENU_MARKED_ID = 1;
        protected const int MENU_SHARE_ID = 2;

        public ProductCardFragment()
            : base(Resource.Layout.app_products_productCard, " ")
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_products_productCard_recyclerView);

            BasketLayout = view.FindViewById<FrameLayout>(Resource.Id.app_products_productCard_basketLayout);

            return view;
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            {
                var markMenuItem = menu.Add(0, MENU_MARKED_ID, 0, new Java.Lang.String(""));

                var typedValue = new TypedValue();
                Activity.Theme.ResolveAttribute((ViewModel?.Marked ?? false ? Resource.Attribute.app_products_ic_marked : Resource.Attribute.app_products_ic_unmarked), typedValue, true);

                var drawable = Activity.Resources.GetDrawable(typedValue.ResourceId, Activity.Theme);
                markMenuItem.SetIcon(drawable);

                markMenuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            }

            {
                var shareMenuItem = menu.Add(0, MENU_SHARE_ID, 0, new Java.Lang.String(""));

                var typedValue = new TypedValue();
                Activity.Theme.ResolveAttribute(Resource.Attribute.app_products_ic_share, typedValue, true);

                var drawable = Activity.Resources.GetDrawable(typedValue.ResourceId, Activity.Theme);
                shareMenuItem.SetIcon(drawable);

                shareMenuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case MENU_MARKED_ID:
                    ViewModel?.MarkCommand.Execute(null);

                    var typedValue = new TypedValue();
                    Activity.Theme.ResolveAttribute((ViewModel?.Marked ?? false ? Resource.Attribute.app_products_ic_marked : Resource.Attribute.app_products_ic_unmarked), typedValue, true);

                    var drawable = Activity.Resources.GetDrawable(typedValue.ResourceId, Activity.Theme);
                    item.SetIcon(drawable);

                    return base.OnOptionsItemSelected(item);
                case MENU_SHARE_ID:
                    ViewModel?.ShareCommand.Execute(null);

                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public abstract class ProductCardFragment<T> : CommonFragment<T>
        where T : class, IProductCardViewModel
    {
        protected MvxRecyclerView RecyclerView { get; set; }

        protected FrameLayout BasketLayout { get; set; }

        protected ProductCardFragment(int layoutId)
            : base(layoutId)
        {
        }

        protected ProductCardFragment(int layoutId, string title)
            : base(layoutId, title)
        {
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            SetupAdapter(recyclerView);
            SetupLayoutManager(recyclerView);
        }

        protected virtual void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARFlatGroupAdapter(null, SetupItemTemplateSelector(), BindingContext)
            {
                TuneSectionItemOnBind = TuneItemOnBind
            };
        }

        protected virtual void TuneItemOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            if (viewHolder.ItemViewType == Resource.Layout.app_products_productCard_imagesCollection)
            {
                var viewPager = viewHolder.ItemView.FindViewById<ARViewPager>(Resource.Id.app_products_productCard_imagesCollection_items);
                var adapter = new ARViewPagerAdapter(Context);
                viewPager.Adapter = adapter;
            }
            else if (viewHolder.ItemViewType == Resource.Layout.app_products_productCard_numberMinMax)
            {
                var fromInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_products_productCard_minMax_content_input_from);
                var toInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_products_productCard_minMax_content_input_to);

                var mvxBindingContextOwner = (viewHolder as IMvxBindingContextOwner);

                BindMinMaxInputs<float>(mvxBindingContextOwner, fromInput, toInput, StringExtentionsMethods.StringPrice);
            }
            else if (viewHolder.ItemViewType == Resource.Layout.app_products_productCard_dateMinMax)
            {
                var fromInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_products_productCard_minMax_content_input_from);
                var toInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_products_productCard_minMax_content_input_to);

                fromInput.Hint = AppSettings.SettingsCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}dd{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}yyyy";
                toInput.Hint = AppSettings.SettingsCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}dd{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}yyyy";

                var mvxBindingContextOwner = (viewHolder as IMvxBindingContextOwner);

                BindMinMaxInputs<DateTime>(mvxBindingContextOwner, fromInput, toInput, StringExtentionsMethods.StringDate);
            }
            else if (viewHolder.ItemViewType == Resource.Layout.app_products_productCard_shopsHorizontalCollection_count ||
                     viewHolder.ItemViewType == Resource.Layout.app_products_productCard_shopsHorizontalCollection_indicator)
            {
                var item = (viewHolder as IMvxBindingContextOwner)?.BindingContext?.DataContext as IHorizontalShopsCollectionPciVm;

                if (item != null)
                {
                    var recyclerView = viewHolder.ItemView.FindViewById<MvxRecyclerView>(Resource.Id.app_products_productCard_shopsHorizontalCollection_recyclerView);
                    recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false));
                    recyclerView.Adapter = new ARSimpleTuneRecyclerAdapter((IMvxAndroidBindingContext)BindingContext)
                    {
                        TuneViewHolderOnCreate = (vH, vT) =>
                        {
                            var lp = vH.ItemView.LayoutParameters ?? new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

                            var metrics = new DisplayMetrics();
                            Activity.WindowManager.DefaultDisplay.GetMetrics(metrics);

                            lp.Width = (int)((metrics.WidthPixels - Resources.GetDimension(Resource.Dimension.app_products_productCard_horizontalMargins) * 2) / 2);

                            vH.ItemView.LayoutParameters = lp;
                        }
                    };
                }
            }
            else if (viewHolder.ItemViewType == Resource.Layout.app_products_productCard_shortInfo)
            {
                var oldPriceTextView = viewHolder.ItemView.FindViewById<TextView>(Resource.Id.app_products_productCard_shortInfo_oldPrice);
                oldPriceTextView.PaintFlags |= Android.Graphics.PaintFlags.StrikeThruText;
            }
        }

        protected virtual void BindMinMaxInputs<TParameter>(IMvxBindingContextOwner mvxBindingContextOwner, EditText fromInput, EditText toInput, Func<object, string> stringFormat)
            where TParameter : IComparable, IComparable<TParameter>, IEquatable<TParameter>
        {
            var set = mvxBindingContextOwner.CreateBindingSet<IMvxBindingContextOwner, IBaseMinMaxPciVm<TParameter>>();

            set.Bind(fromInput).To(vm => vm.Min).WithConversion("StringFormat", new StringFormatParameter { StringFormat = stringFormat });

            set.Bind(toInput).To(vm => vm.Max).WithConversion("StringFormat", new StringFormatParameter { StringFormat = stringFormat });

            set.Apply();
        }

        protected virtual void SetupLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false));
        }

        protected virtual IARFlatGroupTemplateSelector SetupItemTemplateSelector()
        {
            return new ProductCardTemplateSelector();
        }

        protected virtual void SetupBasketLayoutIfExist(FrameLayout basketLayout)
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            var productConfigService = Mvx.Resolve<IProductConfigService>();

            var config = productConfigService.Config;

            if (config.Basket?.AddToCart != null && viewLookupService.IsRegistered(config.Basket?.AddToCart.TypeName))
            {
                var basketView = ViewModel.BasketBlockViewModel == null ? null : Activator.CreateInstance(viewLookupService.Resolve(config.Basket?.AddToCart.TypeName), Context) as IMvxAndroidView;
                if (basketView != null)
                {
                    basketView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(LayoutInflater), ViewModel.BasketBlockViewModel);

                    BasketLayout.AddView((View)basketView);
                }
            }
        }

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupRecyclerView(RecyclerView);

            SetupBasketLayoutIfExist(BasketLayout);
        }
    }
}
