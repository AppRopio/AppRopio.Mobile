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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Messages.Localization;
using AppRopio.Base.Core.Models.App;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Settings.Core.Models;
using AppRopio.Base.Settings.Core.Models.Bundle;
using AppRopio.Base.Settings.Core.Services;
using AppRopio.Base.Settings.Core.ViewModels.Languages.Items;
using AppRopio.Base.Settings.Core.ViewModels.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Base.Settings.Core.ViewModels.Languages
{
    public class LanguagesViewModel : SearchViewModel, ILanguagesViewModel
    {
        #region Fields

        private string _selectedId;

        #endregion

        #region Commands

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand => _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ILangItemVM>(OnItemSelected));

        #endregion

        #region Properties

        public List<ILangItemVM> Items { get; private set; }

        #endregion

        #region Services

        protected SettingsConfig SettingsConfig { get { return Mvx.Resolve<ISettingsConfigService>().Config; } }

        protected AppConfig AppConfig { get { return Mvx.Resolve<AppConfig>(); } }

        public ILangItemVM SelectedLanguage { get; private set; }

        #endregion

        #region Constructor

        public LanguagesViewModel()
        {
            Items = new List<ILangItemVM>();
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameter)
        {
            base.Prepare(parameter);

            var bundle = parameter.ReadAs<SettingsPickerBundle>();
            InitFromBundle(bundle);
        }

        protected virtual void InitFromBundle(SettingsPickerBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;

            _selectedId = parameters.Id;
        }

        #endregion

        protected virtual Task LoadContent()
        {
            return Task.Run(() =>
            {
                Items = AppConfig.Localizations.Select(x => (ILangItemVM)new LangItemVM(x.Key, x.Key == _selectedId)).ToList();
                RaisePropertyChanged(() => Items);
            });
        }

        protected virtual void OnItemSelected(ILangItemVM item)
        {
            SelectedLanguage = item;

            Loading = true;

            AppSettings.SettingsCulture = item.Culture;

            Mvx.Resolve<ILocalizationService>().SetCurrentUICulture(item.Culture);

            Mvx.Resolve<IConnectionService>().Headers["Accept-Language"] = item.Culture.Name;

            Messenger.Publish(new SettingsReloadMessage(this, SettingsElementType.Language, item.Culture.Name, item.Culture.NativeName.ToFirstCharUppercase()));

            Messenger.Publish(new LanguageChangedMessage(this));

            Loading = false;

            Close(this);
        }

        #region Search

        protected override void OnSearchTextChanged(string searchText)
        {

        }

        protected override void SearchCommandExecute()
        {
            Loading = true;

            Items = AppConfig.Localizations
                          .Select(x => (ILangItemVM)new LangItemVM(x.Key, x.Key == _selectedId))
                          .Where(x => x.Name.ToLowerInvariant().Contains(SearchText.ToLowerInvariant()))
                          .ToList();

            RaisePropertyChanged(() => Items);

            Loading = false;
        }

        protected override async void CancelSearchExecute()
        {
            await LoadContent();
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}
