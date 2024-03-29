﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Navigation.Menu.Core.Services;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using MvvmCross.ViewModels;
using MvvmCross;
using AppRopio.Base.Core.Services.Localization;

namespace AppRopio.Navigation.Menu.Core.ViewModels.Services
{
    public class MenuVmService : BaseVmService, IMenuVmService
    {
        #region Fields

        readonly ILocalizationService _localizationService;

        #endregion

        #region Protected

        protected virtual IMenuItemVM SetupItem(string icon, string title, string navigationModelType, bool badge, bool hideBadgeOnNull)
        {
            return new MenuItemVM(icon, title, navigationModelType, badge, hideBadgeOnNull);
        }

        #endregion

        #region Constructor

        public MenuVmService(ILocalizationService localizationService)
        {
            this._localizationService = localizationService;
        }

        #endregion

        #region IMenuVmService implementation

        public Type DefaultViewModelType()
        {
            var config = Mvx.IoCProvider.Resolve<IMenuConfigService>().Config;

            var items = config.Sections.SelectMany(section => section.Items);

            var defaultType = (items.Any(x => x.Default) ?
                               items.First(x => x.Default).Type :
                               items.First().Type);

            var strType = MenuSettings.FirstLaunch ? (config.FirstLaunchType ?? defaultType) : defaultType;

            return Mvx.IoCProvider.Resolve<IViewModelLookupService>().Resolve(strType);
        }

        public IMvxViewModel LoadHeaderVmIfExist()
        {
            var config = Mvx.IoCProvider.Resolve<IMenuConfigService>().Config;

            if (config.Header != null)
            {
                var assembly = Assembly.Load(new AssemblyName(config.Header.AssemblyName));

                var headerType = assembly.GetType(config.Header.TypeName);

                var headerInstance = Activator.CreateInstance(headerType);

                return headerInstance as IMvxViewModel;
            }

            return null;
        }

        public ICollection<IMenuItemVM> LoadItemsFromConfigJSON()
        {
            var config = Mvx.IoCProvider.Resolve<IMenuConfigService>().Config;

            var items = config.Sections.SelectMany(section => section.Items);

            return items.Select(x => SetupItem(x.Icon, _localizationService.GetLocalizableString(MenuConstants.RESX_NAME, x.Type), x.Type, x.Badge, x.HideBadgeOnNull)).ToList();
        }

        public IMvxViewModel LoadFooterVmIfExist()
        {
            var config = Mvx.IoCProvider.Resolve<IMenuConfigService>().Config;

            if (config.Footer != null)
            {
                var assembly = Assembly.Load(new AssemblyName(config.Footer.AssemblyName));

                var footerType = assembly.GetType(config.Footer.TypeName);

                var footerInstance = Activator.CreateInstance(footerType);

                return footerInstance as IMvxViewModel;
            }

            return null;
        }

        #endregion
    }
}

