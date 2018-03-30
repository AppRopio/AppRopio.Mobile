using System;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.ExtensionMethods;

namespace AppRopio.Base.Droid.Adapters
{
    public class ARSimpleTuneRecyclerAdapter : MvxRecyclerAdapter
    {
        public TuneViewHolderOnBindDelegate TuneViewHolderOnBind { get; set; }

        public TuneViewHolderOnCreateDelegate TuneViewHolderOnCreate { get; set; }

        public ARSimpleTuneRecyclerAdapter()
        {
        }

        public ARSimpleTuneRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        public ARSimpleTuneRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            var itemsCount = ItemsSource.Count();

            TuneViewHolderOnBind?.Invoke(position == 0, position == itemsCount, holder);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var viewHolder = base.OnCreateViewHolder(parent, viewType);

            TuneViewHolderOnCreate?.Invoke(viewHolder, viewType);

            return viewHolder;
        }
    }
}
