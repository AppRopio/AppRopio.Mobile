using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Adapters;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Binding.Extensions;
using Android.Widget;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARListView")]
    public class ARListView : MvxListView
    {
        /// <summary>
        /// The default id for the grid header/footer.  This means there is no header/footer
        /// </summary>
        private const int DEFAULT_HEADER_FOOTER_ID = 0;

        private int _footerId = DEFAULT_HEADER_FOOTER_ID;
        private int _headerId = DEFAULT_HEADER_FOOTER_ID;

        public ARListView(Context context, IAttributeSet attrs)
            : base(context, attrs, null)
        {
            IMvxAdapter adapter = new ARListViewAdapter(context);

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;

            ApplyAttributes(context, attrs);

            var headers = GetHeaders();
            var footers = GetFooters();

            IMvxAdapter headerAdapter = new ARHeaderViewListAdapter(headers, footers, adapter);

            Adapter = headerAdapter;
        }

        private void ApplyAttributes(Context c, IAttributeSet attrs)
        {
            _headerId = MvxAttributeHelpers.ReadAttributeValue(c, attrs, Resource.Styleable.ARListView, Resource.Styleable.ARListView_MvxHeaderTemplate);
            _footerId = MvxAttributeHelpers.ReadAttributeValue(c, attrs, Resource.Styleable.ARListView, Resource.Styleable.ARListView_MvxFooterTemplate);
        }

        private IList<FixedViewInfo> GetFixedViewInfos(int id)
        {
            var viewInfos = new List<FixedViewInfo>();

            View view = GetBoundView(id);

            if (view != null)
            {
                var info = new FixedViewInfo(this)
                {
                    Data = null,
                    IsSelectable = true,
                    View = view,
                };
                viewInfos.Add(info);
            }

            return viewInfos;
        }

        private IList<FixedViewInfo> GetFooters()
        {
            return GetFixedViewInfos(_footerId);
        }

        private IList<FixedViewInfo> GetHeaders()
        {
            return GetFixedViewInfos(_headerId);
        }

        private View GetBoundView(int id)
        {
            if (id == DEFAULT_HEADER_FOOTER_ID) return null;

            IMvxAndroidBindingContext bindingContext = MvxAndroidBindingContextHelpers.Current();
            var view = bindingContext.BindingInflate(id, null);

            return view;
        }

        public override Java.Lang.Object GetItemAtPosition(int position)
        {
            var obj =  base.GetItemAtPosition(position);
            return obj;
        }

        protected override void ExecuteCommandOnItem(System.Windows.Input.ICommand command, int position)
        {
            if (_headerId != DEFAULT_HEADER_FOOTER_ID && Adapter is HeaderViewListAdapter headerAdapter)
            {
                if (position < headerAdapter.HeadersCount)
                    return;

                position -= headerAdapter.HeadersCount;
            }
            if (_footerId != DEFAULT_HEADER_FOOTER_ID && position >= ItemsSource.Count())
                return;

            base.ExecuteCommandOnItem(command, position);
        }
    }
}
