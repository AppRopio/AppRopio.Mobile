using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace AppRopio.Base.Droid.Adapters
{
    public class ARLinearLayoutAdapter : MvxAdapterWithChangedEvent, View.IOnClickListener
    {
        public ICommand ItemClick { get; set; }

        public IMvxTemplateSelector ItemTemplateSelector { get; internal set; }

        public ARLinearLayoutAdapter(Context context) 
            : base(context)
        {
        }

        protected ARLinearLayoutAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent, ItemTemplateSelector?.GetItemViewType(GetRawItem(position)) ?? ItemTemplateId);
        }

        protected override View GetView(int position, View convertView, ViewGroup parent, int templateId)
        {
            var view = base.GetView(position, convertView, parent, templateId);

            view.SetOnClickListener(this);

            return view;
        }

        public void OnClick(View view)
        {
            if (view?.Tag is IMvxListItemView mvxListItemView && (ItemClick?.CanExecute(mvxListItemView.DataContext) ?? false))
            {
                ItemClick?.Execute(mvxListItemView.DataContext);
            }
        }
    }
}
