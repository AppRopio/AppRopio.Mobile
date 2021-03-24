﻿using System;
using AppRopio.Base.Contacts.iOS.Models;
using AppRopio.Base.Contacts.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Models.Contacts.Responses;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Contacts.iOS.Views.Contacts
{
    public partial class ContactCell : MvxTableViewCell
    {
		public static readonly NSString Key = new NSString("ContactCell");
		public static readonly UINib Nib;

        protected ContactsThemeConfig ThemeConfig { get { return Mvx.Resolve<IContactsThemeConfigService>().ThemeConfig; } }

		static ContactCell()
		{
			Nib = UINib.FromName("ContactCell", NSBundle.MainBundle);
		}

        public ContactCell(IntPtr handle) : base(handle)
        {
			this.DelayBind(() =>
			{
				InitializeControls();
				BindControls();
			});
        }

		#region InitializationControls

		protected virtual void InitializeControls()
        {
            SetupTitle(TitleLabel);

            this.SetupStyle(ThemeConfig.ContactCell);
		}

		protected virtual void SetupTitle(UILabel name)
		{
			name.SetupStyle(ThemeConfig.ContactCell.Title);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
            var bindingSet = this.CreateBindingSet<ContactCell, ListResponseItem>();

            BindTitle(TitleLabel, bindingSet);
            BindIcon(IconImage, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindTitle(UILabel name, MvxFluentBindingDescriptionSet<ContactCell, ListResponseItem> set)
		{
            set.Bind(name).To(vm => vm.DisplayValue);
		}

        protected virtual void BindIcon(UIImageView image, MvxFluentBindingDescriptionSet<ContactCell, ListResponseItem> set)
		{
            set.Bind(image).To(vm => vm.ImageBASE64).WithConversion("Base64Image");
		}

		#endregion
	}
}