using System;
using UIKit;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Contacts.iOS.Models;
using AppRopio.Base.Contacts.iOS.Services;
using MvvmCross;
using AppRopio.Base.Core;
using AppRopio.Base.iOS;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Contacts.Core;

namespace AppRopio.Base.Contacts.iOS.Views.Contacts
{
    public partial class ContactsViewController : CommonViewController<IContactsViewModel>
    {
        public ContactsViewController()
            : base("ContactsViewController", null)
        {
        }

        #region Private

        private void RegisterCells(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(ContactCell.Nib, ContactCell.Key);
        }

		#endregion

		#region Protected

		protected ContactsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IContactsThemeConfigService>().ThemeConfig; } }

		#region InitializationControls

		protected virtual void SetupTableView(UITableView tableView)
        {
            RegisterCells(tableView);

            tableView.RowHeight = (nfloat)ThemeConfig.ContactCell.Size.Height;
            tableView.TableFooterView = new UIView();
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<ContactsViewController, IContactsViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Contacts);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxStandardTableViewSource(tableView, ContactCell.Key)
            {
                DeselectAutomatically = true
            };
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "Title");

            SetupTableView(contactsTableView);

            if (AppSettings.AppLabel == "contacts")
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
            var bindingSet = this.CreateBindingSet<ContactsViewController, IContactsViewModel>();

            BindTableView(contactsTableView, bindingSet);

            bindingSet.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion

        #region Public

        #endregion
    }
}

