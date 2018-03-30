using System;
using System.Windows.Input;
using Foundation;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.iOS.Views
{
    public class BindableSearchController : UISearchController
    {
        #region Fields

        private bool _firstInit = true;

        #endregion

        #region Commands

        public ICommand CancelCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        #endregion

        public override UISearchBar SearchBar
        {
            get
            {
                var searchBar = base.SearchBar;

                RemoveBottomSeparator();

                return searchBar;
            }
        }

        #region Constructor

        public BindableSearchController()
        {
            DimsBackgroundDuringPresentation = false;
            ObscuresBackgroundDuringPresentation = false;
        }

        public BindableSearchController(UIViewController searchResultsController)
            : base(searchResultsController)
        {
            DimsBackgroundDuringPresentation = false;
            ObscuresBackgroundDuringPresentation = false;
        }

        public BindableSearchController(NSCoder coder)
            : base(coder)
        {
            DimsBackgroundDuringPresentation = false;
            ObscuresBackgroundDuringPresentation = false;
        }

        #endregion

        #region Private

        private void OnCancel(object sender, EventArgs e)
        {
            SearchBar.Text = null;

            ResignFirstResponder();

            if (CancelCommand != null)
                CancelCommand.Execute(null);
        }

        private void OnSearch(object sender, EventArgs e)
        {
            ResignFirstResponder();

            if (SearchCommand != null)
                SearchCommand.Execute(SearchBar.Text);
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (SearchBar != null)
            {
                SearchBar.CancelButtonClicked += OnCancel;
                SearchBar.SearchButtonClicked += OnSearch;
            }
        }

        public void RemoveBottomSeparator()
        {
            var subview = base.SearchBar?.Superview?.Subviews[0];
            if (subview != null)
            {
                if (_firstInit && Theme.ControlPalette.NavigationBar.PrefersLargeTitles)
                    subview.Hidden = subview.Subviews.IsNullOrEmpty();

                if (!subview.Subviews.IsNullOrEmpty())
                {
                    _firstInit = false;
                    subview.Hidden = false;
                    subview.Subviews.ForEach(x => 
                    {
                        x.Hidden = x is UIImageView;
                    });
                }
            }
        }
    }
}
