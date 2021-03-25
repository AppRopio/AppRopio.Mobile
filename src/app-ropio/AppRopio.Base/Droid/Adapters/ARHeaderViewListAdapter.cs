using System;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace AppRopio.Base.Droid.Adapters
{
    public delegate void TuneViewOnCreateDelegate(View view);

    public interface IARHeaderViewListAdapter : IMvxAdapter
    {
        TuneViewOnCreateDelegate TuneHeaderOnCreate { get; set; }

        TuneViewOnCreateDelegate TuneFooterOnCreate { get; set; }
    }

    public class ARHeaderViewListAdapter : HeaderViewListAdapter, IARHeaderViewListAdapter
    {
        private readonly IMvxAdapter _adapter;
        public IMvxAdapter MvxWrappedAdapter => _adapter;

        public TuneViewOnCreateDelegate TuneHeaderOnCreate { get; set; }

        public TuneViewOnCreateDelegate TuneFooterOnCreate { get; set; }

        public ARHeaderViewListAdapter(IList<ListView.FixedViewInfo> headers, IMvxAdapter adapter)
            : this(headers, new List<ListView.FixedViewInfo>(), adapter)
        { }

        public ARHeaderViewListAdapter(IList<ListView.FixedViewInfo> headers, IList<ListView.FixedViewInfo> footers, IMvxAdapter adapter)
            : base(headers, footers, adapter)
        {
            _adapter = adapter;
        }

        public int DropDownItemTemplateId
        {
            get { return _adapter.DropDownItemTemplateId; }
            set { _adapter.DropDownItemTemplateId = value; }
        }

        public IEnumerable ItemsSource
        {
            get { return _adapter.ItemsSource; }
            set { _adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return _adapter.ItemTemplateId; }
            set { _adapter.ItemTemplateId = value; }
        }

        public View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return _adapter.GetDropDownView(position, convertView, parent);
        }

        public int GetPosition(object value)
        {
            return _adapter.GetPosition(value);
        }

        public object GetRawItem(int position)
        {
            return _adapter.GetRawItem(position);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);

            if (position < HeadersCount)
            {
                TuneHeaderOnCreate?.Invoke(view);
            }
            if (position >= HeadersCount + _adapter.Count)
            {
                TuneFooterOnCreate?.Invoke(view);
            }

            return view;
        }
    }

    public class ARListViewAdapter : MvxAdapter
    {
        public delegate void TuneItemViewOnCreateDelegate(object dataContext, IMvxListItemView itemView);

        public TuneItemViewOnCreateDelegate TuneItemViewOnCreate { get; set; }
        
        public ARListViewAdapter(Context context) 
            : base(context)
        {
        }

        public ARListViewAdapter(Context context, IMvxAndroidBindingContext bindingContext) 
            : base(context, bindingContext)
        {
        }

        protected ARListViewAdapter(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, ViewGroup parent, int templateId)
        {
            var itemView = base.CreateBindableView(dataContext, parent, templateId);

            TuneItemViewOnCreate?.Invoke(dataContext, itemView);

            return itemView;
        }
    }
}
