//
//  Copyright 2018  AppRopio
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;

using UIKit;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.iOS;
using AppRopio.Base.Settings.iOS.Models;
using MvvmCross.Platform;
using AppRopio.Base.Settings.iOS.Services;
using CoreGraphics;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Settings.iOS.Views.Languages.Cells;
using MvvmCross.Binding.iOS.Views;

namespace AppRopio.Base.Settings.iOS.Views.Languages
{
    public partial class LanguagesViewController : CommonViewController<ILanguagesViewModel>
    {
        private BindableSearchBar _searchBar;
        private BindableSearchController _searchController;

        protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

        public LanguagesViewController()
            : base("LanguagesViewController", null)
        {
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILanguagesViewModel.SelectedLanguage))
            {
                _searchController?.SearchBar?.EndEditing(true);
                _searchController?.DismissViewControllerAsync(true);
                _searchBar?.EndEditing(true);
            }
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "Регион";

            DefinesPresentationContext = true;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _searchController = new BindableSearchController(searchResultsController: null);

                NavigationItem.HidesSearchBarWhenScrolling = true;
                NavigationItem.SearchController = _searchController;

                SetupSearchBar(_searchController.SearchBar);
            }
            else
            {
                _searchBar = new BindableSearchBar(new CGRect(8, 0, DeviceInfo.ScreenWidth - 16, 44));

                SetupSearchBar(_searchBar);

                TableView.TableHeaderView = new UIView()
                    .WithFrame(0, 0, DeviceInfo.ScreenWidth, 44)
                    .WithSubviews(_searchBar);
            }

            SetupTableView(TableView);
        }

        protected virtual void SetupSearchBar(UISearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.Regions.SearchBar);
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(LanguageCell.Nib, LanguageCell.Key);

            tableView.RowHeight = (nfloat)ThemeConfig.Regions.RegionCell.Size.Height;
            tableView.TableFooterView = new UIView();
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<LanguagesViewController, ILanguagesViewModel>();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                BindSearchController(_searchController, set);
            else
                BindSearchBar(_searchBar, set);

            BindTable(TableView, set);

            set.Apply();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            ReleaseDesignerOutlets();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationItem.SearchController != null)
                NavigationItem.SearchController = null;
        }

        #endregion

        #region BingingControls

        protected virtual void BindSearchController(BindableSearchController searchController, MvxFluentBindingDescriptionSet<LanguagesViewController, ILanguagesViewModel> set)
        {
            set.Bind(searchController.SearchBar).To(vm => vm.SearchText);
            set.Bind(searchController).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchController).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<LanguagesViewController, ILanguagesViewModel> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindTable(UITableView tableView, MvxFluentBindingDescriptionSet<LanguagesViewController, ILanguagesViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(d => d.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, LanguageCell.Key)
            {
                DeselectAutomatically = true
            };
        }

        #endregion

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _searchController?.RemoveBottomSeparator();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _searchController?.RemoveBottomSeparator();
        }
    }
}

