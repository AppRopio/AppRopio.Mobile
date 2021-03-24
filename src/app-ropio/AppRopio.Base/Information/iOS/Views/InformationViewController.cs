using System;
using AppRopio.Base.Core;
using AppRopio.Base.Information.Core.ViewModels.Information;
using AppRopio.Base.Information.iOS.Models;
using AppRopio.Base.Information.iOS.Services;
using AppRopio.Base.Information.iOS.Views.Cell;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.Information.Core;

namespace AppRopio.Base.Information.iOS.Views
{
    public partial class InformationViewController : CommonViewController<IInformationViewModel>
    {
        protected InformationThemeConfig ThemeConfig { get { return Mvx.Resolve<IInformationThemeConfigService>().ThemeConfig; } }

        public InformationViewController() 
            : base("InformationViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(InformationConstants.RESX_NAME, "Title");

            SetupTableView(TableView);

            if (AppSettings.AppLabel == "information")
            {
                var appropioView = new AppRopioView();
                _tableViewBottomConstraint.Constant = appropioView.Bounds.Height;

                View.AddSubview(appropioView);

                View.AddConstraints(new NSLayoutConstraint[]
                {
                    NSLayoutConstraint.Create(appropioView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, View, NSLayoutAttribute.Leading, 1, 0),
                    NSLayoutConstraint.Create(appropioView, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, View, NSLayoutAttribute.Trailing, 1, 0),
                    NSLayoutConstraint.Create(appropioView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, 0),
                });
            }
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<InformationViewController, IInformationViewModel>();

            BindTableView(TableView, bindingSet);

            bindingSet.Apply();
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(ArticleCell.Nib, ArticleCell.Key);

            tableView.RowHeight = (nfloat)ThemeConfig.InformationCell.Size.Height;
            tableView.TableFooterView = new UIView();
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<InformationViewController, IInformationViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Articles);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxStandardTableViewSource(tableView, ArticleCell.Key)
            {
                DeselectAutomatically = true
            };
        }

        #endregion
    }
}