using System;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items
{
    public class CategoriesItemVM : MvxViewModel, ICategoriesItemVM
    {
        #region Properties

        public string CategoryID { get; protected set; }

        public string Name { get; protected set; }

        public string IconUrl { get; protected set; }

        public string BackgroundImageUrl { get; protected set; }

        public bool IsFolder { get; protected set; }

        #endregion

        #region Constructor

        public CategoriesItemVM(string id, string name, string iconUrl, string backgroundImageUrl, bool isFolder)
        {
            CategoryID = id;
            Name = name;
            IconUrl = iconUrl;
            BackgroundImageUrl = backgroundImageUrl;
            IsFolder = isFolder;
        }

        #endregion
    }
}

