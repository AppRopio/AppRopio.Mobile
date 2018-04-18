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
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.Settings.Core.ViewModels.Languages.Items;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Languages.Cells
{
    public partial class LanguageCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LanguageCell");
        public static readonly UINib Nib;

        protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

        static LanguageCell()
        {
            Nib = UINib.FromName("LanguageCell", NSBundle.MainBundle);
        }

        protected LanguageCell(IntPtr handle) 
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region IntializationControls

        protected virtual void InitializeControls()
        {
            SetupTitle(TitleLabel);
            SetupSelectionImage(SelectionImageView);

            this.SetupStyle(ThemeConfig.Regions.RegionCell);
        }

        protected virtual void SetupTitle(UILabel title)
        {
            title.SetupStyle(ThemeConfig.Regions.RegionCell.Title);
        }

        protected virtual void SetupSelectionImage(UIImageView selectionImageView)
        {
            selectionImageView.Image = ImageCache.GetImage("Images/Settings/Selection.png");
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var bindingSet = this.CreateBindingSet<LanguageCell, ILangItemVM>();

            BindTitle(TitleLabel, bindingSet);

            BindSelectionImage(SelectionImageView, bindingSet);

            bindingSet.Apply();
        }

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<LanguageCell, ILangItemVM> set)
        {
            set.Bind(title).To(vm => vm.Name);
        }

        protected virtual void BindSelectionImage(UIImageView selectionImageView, MvxFluentBindingDescriptionSet<LanguageCell, ILangItemVM> set)
        {
            set.Bind(selectionImageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion
    }
}
