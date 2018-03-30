using System;
namespace AppRopio.Navigation.Menu.Core.ViewModels.Items
{
    public interface IMenuItemVM
    {
        string Icon { get; }

        string Type { get; }

        string Title { get; }

        bool BadgeEnabled { get; set; }

        int BadgeCount { get; set; }
    }
}

