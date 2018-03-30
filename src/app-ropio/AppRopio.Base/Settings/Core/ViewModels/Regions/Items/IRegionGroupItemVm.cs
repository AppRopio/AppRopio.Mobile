using System;
using System.Collections.Generic;

namespace AppRopio.Base.Settings.Core.ViewModels.Regions.Items
{
    public interface IRegionGroupItemVm
    {
		string Title { get; set; }

		List<IRegionItemVm> Items { get; set; }
    }
}