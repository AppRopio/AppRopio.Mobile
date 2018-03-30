using System;
using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Regions.Items
{
    public class RegionGroupItemVm : BaseViewModel, IRegionGroupItemVm
    {
        public string Title { get; set; }

        public List<IRegionItemVm> Items { get; set; }
    }
}