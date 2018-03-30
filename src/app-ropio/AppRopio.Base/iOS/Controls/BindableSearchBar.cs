using System;
using System.ComponentModel;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS
{
    [Register("BindableSearchBar"), DesignTimeVisible(true)]
    public class BindableSearchBar : UISearchBar
    {
        private bool _isShowCancelOnEditing;
        public bool IsShowCancelOnEditing
        {
            get
            {
                return _isShowCancelOnEditing;
            }
            set
            {
                _isShowCancelOnEditing = value;
                if (value)
                {
                    OnEditingStarted += ShowCancel;
                    OnEditingStopped += HideCancel;
                }
                else
                {
                    OnEditingStarted -= ShowCancel;
                    OnEditingStopped -= HideCancel;
                }
            }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public BindableSearchBar(CGRect frame)
            : base(frame)
        {
            CancelButtonClicked += OnCancel;
            SearchButtonClicked += OnSearch;
        }

        public BindableSearchBar(IntPtr handle)
            : base(handle)
        {
            CancelButtonClicked += OnCancel;
            SearchButtonClicked += OnSearch;
        }

        private void ShowCancel(object sender, EventArgs e)
        {
            SetShowsCancelButton(true, true);
        }

        private void HideCancel(object sender, EventArgs e)
        {
            if (ShowsCancelButton)
            {
                SetShowsCancelButton(false, true);
                if (string.IsNullOrEmpty(Text))
                    OnCancel(sender, e);
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Text = null;
            ResignFirstResponder();
            if (CancelCommand != null)
                CancelCommand.Execute(null);
        }

        private void OnSearch(object sender, EventArgs e)
        {
            ResignFirstResponder();
            if (SearchCommand != null)
                SearchCommand.Execute(Text);
        }
    }
}

