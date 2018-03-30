using System;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Binding.ExtensionMethods;

namespace AppRopio.Base.Droid.Adapters
{
    public class ARPagingFlatGroupRecyclerAdapter : ARFlatGroupAdapter
    {
        protected int LoadMoreOffset { get; set; } = 1;
        
        protected ILoadMoreViewModel ViewModel { get; }

        public ARPagingFlatGroupRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public ARPagingFlatGroupRecyclerAdapter(ILoadMoreViewModel viewModel, InnerItemsProviderDelegate innerItemsProvider, IARFlatGroupTemplateSelector flatGroupTemplateSelector, IMvxBindingContext bindingContext)
            
            : base(innerItemsProvider, flatGroupTemplateSelector, bindingContext)
        {
            ViewModel = viewModel;
        }

        public ARPagingFlatGroupRecyclerAdapter(ILoadMoreViewModel viewModel, InnerItemsProviderDelegate innerItemsProvider, IMvxBindingContext bindingContext, int sectionHeaderLayout = -1, int sectionFooterLayout = -1) 
            : base(innerItemsProvider, bindingContext, sectionHeaderLayout, sectionFooterLayout)
        {
            ViewModel = viewModel;
        }

        protected ARPagingFlatGroupRecyclerAdapter(ILoadMoreViewModel viewModel, InnerItemsProviderDelegate innerItemsProvider, IMvxAndroidBindingContext bindingContext)
            : base(innerItemsProvider, bindingContext)
        {
            ViewModel = viewModel;
        }

        public override object GetItem(int position)
        {
            if ((position >= this.ItemsSource.Count() - LoadMoreOffset - FootersCount) && 
                ViewModel.CanLoadMore && 
                !ViewModel.LoadingMore && 
                (ViewModel.LoadMoreCommand?.CanExecute(null) ?? false))
            {
                ViewModel.LoadMoreCommand?.Execute();
            }

            return base.GetItem(position);
        }
    }
}
